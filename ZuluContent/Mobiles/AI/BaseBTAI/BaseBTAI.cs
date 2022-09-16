using System;
using Server.Utilities;

namespace Server.Mobiles;

public class BaseBTAI
{
    private Timer m_Timer;
    private BTTree BehaviorTree { get; }
    
    public BaseCreature Mobile { get; }

    public BaseBTAI(BaseCreature mobile, Type behaviorTreeType)
    {
        BehaviorTree = behaviorTreeType.CreateInstance<BTTree>(this);
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
            BehaviorTree.Start();
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
                    if (!Mobile.Region.AcceptsSpawnsFrom(se.Region))
                    {
                        Timer.DelayCall(TimeSpan.Zero, ReturnToHome);
                    }
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
    
    private void ReturnToHome()
    {
        if (Mobile.Spawner != null)
        {
            Point3D loc = Mobile.Spawner.GetSpawnPosition(Mobile, Mobile.Spawner.Map);

            if (loc != Point3D.Zero) Mobile.MoveToWorld(loc, Mobile.Spawner.Map);
        }
    }

    private class AITimer : Timer
    {
        private BaseBTAI m_AI;

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