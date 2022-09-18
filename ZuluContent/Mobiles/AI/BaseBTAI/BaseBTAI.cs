using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.Utilities;
using MoveImpl = Server.Movement.MovementImpl;

namespace Server.Mobiles;

public class BaseBTAI
{
    private readonly Timer m_Timer;
    private BTTree BehaviorTree { get; }

    public BaseCreature Mobile { get; }

    public BaseBTAI(BaseCreature mobile, BTTree behaviorTree)
    {
        BehaviorTree = behaviorTree;
        Mobile = mobile;
        m_Timer = new AITimer(this);

        bool activate;

        if (!mobile.PlayerRangeSensitive)
            activate = true;
        else if (World.Loading)
            activate = false;
        else if (mobile.Map == null || mobile.Map == Map.Internal || !mobile.Map.GetSector(mobile.Location).Active)
            activate = false;
        else
            activate = true;

        if (activate)
        {
            m_Timer.Start();
        }
    }

    public void Deactivate()
    {
        if (Mobile.PlayerRangeSensitive)
        {
            m_Timer.Stop();

            var se = Mobile.Spawner;

            if (se != null && se.ReturnOnDeactivate && !Mobile.Controlled)
            {
                if (se.HomeLocation == Point3D.Zero)
                {
                    if (!Mobile.Region.AcceptsSpawnsFrom(se.Region)) Timer.DelayCall(TimeSpan.Zero, ReturnToHome);
                }
                else if (!Mobile.InRange(se.HomeLocation, se.HomeRange))
                {
                    Timer.DelayCall(TimeSpan.Zero, ReturnToHome);
                }
            }
        }
        else if (Mobile.Deleted)
        {
            m_Timer.Stop();
        }
    }

    public void Activate()
    {
        if (!m_Timer.Running)
        {
            m_Timer.Delay = TimeSpan.Zero;
            m_Timer.Start();
        }
    }
    
    public void StopTimer()
    {
        m_Timer.Stop();
    }
    
    public void OnAggressiveAction(Mobile aggressor)
    {
        var currentCombat = Mobile.Combatant;

        if (currentCombat != null && !aggressor.Hidden && currentCombat != aggressor &&
            Mobile.GetDistanceToSqrt(currentCombat) > Mobile.GetDistanceToSqrt(aggressor))
            Mobile.Combatant = aggressor;
    }

    private void ReturnToHome()
    {
        if (Mobile.Spawner != null)
        {
            var loc = Mobile.Spawner.GetSpawnPosition(Mobile, Mobile.Spawner.Map);

            if (loc != Point3D.Zero) Mobile.MoveToWorld(loc, Mobile.Spawner.Map);
        }
    }

    public static bool AcquireFocusMob(BaseCreature mobile, int iRange, FightMode acqType, bool bPlayerOnly, bool bFacFriend,
        bool bFacFoe)
    {
        if (mobile.Deleted)
            return false;

        if (mobile.BardProvoked)
        {
            if (mobile.BardTarget == null || mobile.BardTarget.Deleted)
            {
                mobile.FocusMob = null;
                return false;
            }

            mobile.FocusMob = mobile.BardTarget;
            return mobile.FocusMob != null;
        }

        if (mobile.Controlled)
        {
            if (mobile.ControlTarget == null || mobile.ControlTarget.Deleted || mobile.ControlTarget.Hidden ||
                !mobile.ControlTarget.Alive ||
                !mobile.InRange(mobile.ControlTarget, mobile.RangePerception * 2))
            {
                if (mobile.ControlTarget != null && mobile.ControlTarget != mobile.ControlMaster)
                    mobile.ControlTarget = null;

                mobile.FocusMob = null;
                return false;
            }

            mobile.FocusMob = mobile.ControlTarget;
            return mobile.FocusMob != null;
        }

        if (mobile.ConstantFocus != null)
        {
            mobile.DebugSay("Acquired my constant focus");
            mobile.FocusMob = mobile.ConstantFocus;
            return true;
        }

        if (acqType == FightMode.None)
        {
            mobile.FocusMob = null;
            return false;
        }

        if (acqType == FightMode.Aggressor && mobile.Aggressors.Count == 0 && mobile.Aggressed.Count == 0)
        {
            mobile.FocusMob = null;
            return false;
        }

        if (mobile.NextReacquireTime > Core.TickCount)
        {
            mobile.FocusMob = null;
            return false;
        }

        mobile.NextReacquireTime = Core.TickCount + (int)mobile.ReacquireDelay.TotalMilliseconds;

        mobile.DebugSay("Acquiring...");

        var map = mobile.Map;

        if (map != null)
        {
            Mobile newFocusMob = null;
            var val = double.MinValue;
            double theirVal;

            IPooledEnumerable eable = map.GetMobilesInRange(mobile.Location, iRange);

            foreach (Mobile m in eable)
            {
                if (m.Deleted || m.Blessed)
                    continue;

                // Let's not target ourselves...
                if (m == mobile)
                    continue;

                // We consider Familiars to be neutral
                if (m is BaseCreature { AI: AIType.AI_Familiar })
                    continue;

                // Dead targets are invalid.
                if (!m.Alive)
                    continue;

                // Staff members cannot be targeted.
                if (m.AccessLevel > AccessLevel.Player)
                    continue;

                // Does it have to be a player?
                if (bPlayerOnly && !m.Player)
                    continue;

                // Can't acquire a target we can't see.
                if (!mobile.CanSee(m))
                    continue;

                if (mobile.Summoned && mobile.SummonMaster != null)
                {
                    // If this is a summon, it can't target its controller.
                    if (m == mobile.SummonMaster)
                        continue;

                    // It also must abide by harmful spell rules.
                    if (!SpellHelper.ValidIndirectTarget(mobile.SummonMaster, m))
                        continue;
                }

                // If we only want faction friends, make sure it's one.
                if (bFacFriend && !mobile.IsFriend(m))
                    continue;

                if (acqType == FightMode.Aggressor || acqType == FightMode.Evil)
                {
                    bool bValid = IsHostile(mobile, m);

                    if (acqType == FightMode.Evil && !bValid)
                    {
                        if (m is BaseCreature && ((BaseCreature)m).Controlled &&
                            ((BaseCreature)m).ControlMaster != null)
                            bValid = ((BaseCreature)m).ControlMaster.Karma < 0;
                        else
                            bValid = m.Karma < 0;
                    }

                    if (!bValid)
                        continue;
                }
                else
                {
                    // Same goes for faction enemies.
                    if (bFacFoe && !mobile.IsEnemy(m))
                        continue;

                    // If it's an enemy factioned mobile, make sure we can be harmful to it.
                    if (bFacFoe && !bFacFriend && !mobile.CanBeHarmful(m, false))
                        continue;
                }

                theirVal = mobile.GetFightModeRanking(m, acqType, bPlayerOnly);

                if (theirVal > val && mobile.InLOS(m))
                {
                    newFocusMob = m;
                    val = theirVal;
                }
            }

            eable.Free();

            mobile.FocusMob = newFocusMob;
        }

        var focusedMob = mobile.FocusMob != null;

        if (mobile.Hidden && focusedMob)
            Timer.StartTimer(TimeSpan.FromMilliseconds(100), () => AlertUnhidden(mobile));

        return focusedMob;
    }
    
    public static bool IsHostile(Mobile to, Mobile from)
    {
        int count = Math.Max(to.Aggressors.Count, to.Aggressed.Count);

        if (to.Combatant == from || from.Combatant == to)
        {
            return true;
        }

        if (count > 0)
        {
            for (int a = 0; a < count; ++a)
            {
                if (a < to.Aggressed.Count && to.Aggressed[a].Attacker == from)
                {
                    return true;
                }

                if (a < to.Aggressors.Count && to.Aggressors[a].Defender == from)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static void AlertUnhidden(Mobile mobile)
    {
        mobile.Say($"*{mobile.Name} springs out from hiding!*");
    }
    
    private static readonly Queue<Item> Obstacles = new();

    public static bool CheckMove(long nextMove) => Core.TickCount - nextMove >= 0;

    public static double TransformMoveDelay(BaseCreature mobile, double delay)
    {
        if (mobile is { Controlled: false, Summoned: false } && Math.Abs(delay - mobile.PassiveSpeed) < 0.0001)
        {
            delay *= 3;
        }
        
        var stats = mobile.Hits;
        var statsMax = mobile.HitsMax;

        var offset = statsMax <= 0 ? 1.0 : Math.Max(0, stats) / (double)statsMax;

        if (offset < 1.0)
        {
            delay += mobile.PassiveSpeed * (1.0 - offset);
        }

        return delay;
    }
    
    public static MoveResult DoMoveImpl(BaseCreature mobile, Direction d, ref long nextMove)
    {
        if (mobile.Deleted || mobile.Frozen || mobile.Paralyzed ||
            mobile.Spell?.IsCasting == true || mobile.DisallowAllMoves)
        {
            return MoveResult.BadState;
        }

        if (!CheckMove(nextMove))
        {
            return MoveResult.BadState;
        }

        // This makes them always move one step, never any direction changes
        // TODO: This is firing off deltas which aren't needed. Look into replacing/removing this
        mobile.Direction = d;

        var delay = (int)(TransformMoveDelay(mobile, mobile.CurrentSpeed) * 1000);
        nextMove += delay;

        if (Core.TickCount - nextMove > 0)
        {
            nextMove = Core.TickCount;
        }

        mobile.Pushing = false;

        MoveImpl.IgnoreMovableImpassables = mobile.CanMoveOverObstacles && !mobile.CanDestroyObstacles;

        if ((mobile.Direction & Direction.Mask) != (d & Direction.Mask))
        {
            var v = mobile.Move(d);

            MoveImpl.IgnoreMovableImpassables = false;
            return v ? MoveResult.Success : MoveResult.Blocked;
        }

        if (mobile.Move(d))
        {
            MoveImpl.IgnoreMovableImpassables = false;
            return MoveResult.Success;
        }

        var wasPushing = mobile.Pushing;

        var blocked = true;

        var canOpenDoors = mobile.CanOpenDoors;
        var canDestroyObstacles = mobile.CanDestroyObstacles;

        if (canOpenDoors || canDestroyObstacles)
        {
            if (mobile.Debug)
            {
                mobile.DebugSay("My movement was blocked, I will try to clear some obstacles.");
            }

            var map = mobile.Map;

            if (map != null)
            {
                int x = mobile.X, y = mobile.Y;
                Movement.Movement.Offset(d, ref x, ref y);

                var destroyables = 0;

                var eable = map.GetItemsInRange(new Point3D(x, y, mobile.Location.Z), 1);

                foreach (var item in eable)
                {
                    if (canOpenDoors && item is BaseDoor door && door.Z + door.ItemData.Height > mobile.Z &&
                        mobile.Z + 16 > door.Z)
                    {
                        if (door.X != x || door.Y != y)
                        {
                            continue;
                        }

                        if (!door.Locked || !door.UseLocks())
                        {
                            Obstacles.Enqueue(door);
                        }

                        if (!canDestroyObstacles)
                        {
                            break;
                        }
                    }
                    else if (canDestroyObstacles && item.Movable && item.ItemData.Impassable &&
                             item.Z + item.ItemData.Height > mobile.Z && mobile.Z + 16 > item.Z)
                    {
                        if (!mobile.InRange(item.GetWorldLocation(), 1))
                        {
                            continue;
                        }

                        Obstacles.Enqueue(item);
                        ++destroyables;
                    }
                }

                eable.Free();

                if (destroyables > 0)
                {
                    Effects.PlaySound(new Point3D(x, y, mobile.Z), mobile.Map, 0x3B3);
                }

                if (Obstacles.Count > 0)
                {
                    blocked = false; // retry movement
                }

                while (Obstacles.Count > 0)
                {
                    var item = Obstacles.Dequeue();

                    if (item is BaseDoor door)
                    {
                        if (mobile.Debug)
                        {
                            mobile.DebugSay(
                                "Little do they expect, I've learned how to open doors. Didn't they read the script??"
                            );
                        }

                        if (mobile.Debug)
                        {
                            mobile.DebugSay("*twist*");
                        }

                        door.Use(mobile);
                    }
                    else
                    {
                        if (mobile.Debug)
                        {
                            mobile.DebugSay(
                                $"Ugabooga. I'm so big and tough I can destroy it: {item.GetType().Name}"
                            );
                        }

                        if (item is Container cont)
                        {
                            for (var i = 0; i < cont.Items.Count; ++i)
                            {
                                var check = cont.Items[i];

                                if (check.Movable && check.ItemData.Impassable &&
                                    cont.Z + check.ItemData.Height > mobile.Z)
                                {
                                    Obstacles.Enqueue(check);
                                }
                            }

                            cont.Destroy();
                        }
                        else
                        {
                            item.Delete();
                        }
                    }
                }

                if (!blocked)
                {
                    blocked = !mobile.Move(d);
                }
            }
        }

        if (blocked)
        {
            var offset = Utility.RandomDouble() >= 0.6 ? 1 : -1;

            for (var i = 0; i < 2; ++i)
            {
                mobile.TurnInternal(offset);

                if (mobile.Move(mobile.Direction))
                {
                    MoveImpl.IgnoreMovableImpassables = false;
                    return MoveResult.SuccessAutoTurn;
                }
            }

            MoveImpl.IgnoreMovableImpassables = false;
            return wasPushing ? MoveResult.BadState : MoveResult.Blocked;
        }

        MoveImpl.IgnoreMovableImpassables = false;
        return MoveResult.Success;
    }

    private class AITimer : Timer
    {
        private readonly BaseBTAI m_AI;

        public AITimer(BaseBTAI ai)
            : base(TimeSpan.FromSeconds(Utility.RandomDouble()),
                TimeSpan.FromSeconds(Math.Max(0.0, ai.Mobile.CurrentSpeed)))
        {
            m_AI = ai;
        }

        protected override void OnTick()
        {
            var state = m_AI.BehaviorTree.Update();

            if (state == NodeState.FAILURE)
                m_AI.Deactivate();
        }
    }
}