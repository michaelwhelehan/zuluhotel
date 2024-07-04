using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles;

public class CountDracula : BaseBoss
{
    public override Rectangle2D Bounds { get; } = new(5566, 1847, 25, 25);

    private long NextVampireThrowTime { get; set; }

    private long NextBloodShower { get; set; }
    
    private CountDraculaChest Chest { get; set; }

    private const int WaterfallEw = 13591;
    private const int WaterfallNs = 13561;

    private static readonly int[] Pool = { 6054, 6047, 6053, 6051, 6039, 6045, 6056, 6049, 6055 };

    [Constructible]
    public CountDracula(string templateName) : base(templateName)
    {
    }

    public CountDracula(Serial serial) : base(serial)
    {
    }

    public override void OnBossPhaseThink()
    {
        base.OnBossPhaseThink();

        if (Warmode && Combatant != null && BossPhase == 1)
        {
            Say("Who dares such stupidity?!");
            PlaySound(0x448);
            Combatant = null;
            Warmode = false;
            RemoveAggressor(LastAggressor);

            IsInvulnerable = true;
            DisallowAllMoves = false;

            TargetLocation = new Point2D(5577, 1849);

            BossPhase = 2;
        }

        switch (BossPhase)
        {
            case 2:
            {
                if (Location.X == 5577 && Location.Y == 1849)
                {
                    DisallowAllMoves = true;
                    Direction = Direction.West;
                    
                    IPooledEnumerable eable = Map.GetItemsInRange(Location, 1);

                    foreach (Item item in eable)
                        if (item is CountDraculaChest draculaChest)
                        {
                            Chest = draculaChest;
                            break;
                        }

                    eable.Free();
                    
                    BossPhase = 3;

                    var index = 0;
                    Timer.StartTimer(TimeSpan.Zero, TimeSpan.FromSeconds(0.5), 5,
                        () => GetDressed(index++));
                }

                break;
            }
            case 4:
            {
                Name = "Count Dracula";
                Say("Ah, you again...");
                PlaySound(0x431);
                TargetLocation = null;
                DisallowAllMoves = false;
                IsInvulnerable = false;

                AggressiveAction(LastAggressor, false);

                CheckReset = true;

                BossPhase = 5;
                break;
            }
            case 5:
            {
                if (Core.TickCount > NextVampireThrowTime)
                {
                    if (Core.TickCount - NextVampireThrowTime <= (int)TimeSpan.FromSeconds(40.0).TotalMilliseconds)
                    {
                        Say("Arise minions! Do my bidding!");
                        PlaySound(0x481);
                        Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromMilliseconds(100), 2, ThrowVampire);
                    }
                    
                    NextVampireThrowTime = Core.TickCount +
                                                (int)TimeSpan.FromSeconds(40.0).TotalMilliseconds;
                }

                if (Core.TickCount > NextBloodShower)
                {
                    if (Core.TickCount - NextBloodShower < (int)TimeSpan.FromSeconds(15.0).TotalMilliseconds)
                    {
                        Say("Get doused with blood!");
                        Timer.StartTimer(TimeSpan.Zero, TimeSpan.FromMilliseconds(100), 15, () =>
                            ShowerBlood(new Point3D(Location.X + Utility.RandomMinMax(-6, 6),
                                Location.Y + Utility.RandomMinMax(-6, 6),
                                Location.Z)));
                    }

                    NextBloodShower = Core.TickCount +
                                      (int)TimeSpan.FromSeconds(15.0).TotalMilliseconds;
                }

                if (Hits <= HitsMax / 4)
                {
                    Say($"*{Name} assumes his final form!*");
                    Body = 317;
                    FixedParticles(0x373A, 10, 10, 5007, EffectLayer.Waist);
                    PlaySound(0x209);
                    
                    RawStr = (int) (RawStr * 1.15);
                    
                    BossPhase = 6;
                }

                break;
            }
            case 6:
            {
                Timer.StartTimer(TimeSpan.Zero, TimeSpan.FromSeconds(1.0), () =>
                    ShowerBlood(new Point3D(Location.X + Utility.RandomMinMax(-6, 6),
                        Location.Y + Utility.RandomMinMax(-6, 6),
                        Location.Z)));

                BossPhase = 7;

                break;
            }
        }
    }

    private async void ThrowVampire()
    {
        if (!Alive || Deleted)
            return;
        
        var map = Map;
        var fromLocation = Location;
        var toLocation = new Point3D(fromLocation.X + Utility.RandomMinMax(-3, 3),
            fromLocation.Y + Utility.RandomMinMax(-3, 3),
            fromLocation.Z);

        if (!map.LineOfSight(this, toLocation))
            return;

        Effects.SendMovingEffect(map, 0x1CE0, fromLocation, toLocation, 10, 30);

        Timer.Pause(100);

        Summon("Vampire", false, this, toLocation, 0x212, TimeSpan.FromMinutes(5));
    }

    private async void ShowerBlood(Point3D targetLocation)
    {
        if (!Alive || Deleted)
            return;
        
        var point = SpellHelper.GetSurfaceTop(targetLocation);

        PlaySound(0x026);

        // Blood column
        for (var z = 10; z >= 0; z--)
        {
            var loc = new Point3D(point.X, point.Y, point.Z + z * 10);

            var fall1 = EffectItem.Create(loc, Map, TimeSpan.FromSeconds(5));
            fall1.ItemID = WaterfallEw;
            fall1.Hue = 36;
            var fall2 = EffectItem.Create(loc, Map, TimeSpan.FromSeconds(5));
            fall2.ItemID = WaterfallNs;
            fall2.Hue = 36;
            await Timer.Pause(50);
        }

        // Blood pool
        var poolNum = 0;
        for (var y = -1; y <= 1; y++)
        {
            for (var x = -1; x <= 1; x++)
            {
                var loc = new Point3D(point.X + x, point.Y + y, point.Z);
                var poolItem = EffectItem.Create(loc, Map, TimeSpan.FromSeconds(10));
                poolItem.ItemID = Pool[poolNum];
                poolItem.Hue = 36;
                poolNum++;
            }
        }

        Effects.PlaySound(point, Map, 0x10);

        Timer.StartTimer(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 8,
            () => DamageFromBlood(targetLocation, Map));
    }

    private static void DamageFromBlood(Point3D bloodLocation, Map map)
    {
        if (map == null)
            return;
        
        var eable = map.GetMobilesInRange(bloodLocation, 1);
        foreach (var target in eable)
        {
            if (target is not PlayerMobile)
                continue;

            Effects.PlaySound(bloodLocation, target.Map, 0x1F1);
            target.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);
            target.Damage(Utility.RandomMinMax(30, 50));
            target.PrivateOverheadMessage(MessageType.Regular,
                0x3B2,
                true,
                "*The tainted blood damages you*",
                target.NetState);
        }

        eable.Free();
    }

    private void GetDressed(int index)
    {
        switch (index)
        {
            case 1:
            {
                Say("Let me quickly gather my things...");
                var cloak = new Cloak
                {
                    Hue = 1
                };
                EquipItem(cloak);
                cloak.Movable = false;
                PlaySound(0x057);
                Animate(32, 5, 1, true, false, 0);
                break;
            }
            case 2:
            {
                var shirt = new FancyShirt();
                EquipItem(shirt);
                shirt.Movable = false;
                PlaySound(0x057);
                Animate(32, 5, 1, true, false, 0);
                break;
            }
            case 3:
            {
                var pants = new LongPants
                {
                    Hue = 1
                };
                EquipItem(pants);
                pants.Movable = false;
                PlaySound(0x057);
                Animate(32, 5, 1, true, false, 0);
                break;
            }
            case 4:
            {
                BossPhase = 4;
                break;
            }
        }
    }

    public override void OnDeath(Container c)
    {
        base.OnDeath(c);

        if (Chest != null)
        {
            Chest.TryDropItems(this, false, c.Items.ToArray());
            Chest.Locked = false;
            Chest.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The chest pops open!*");
        }
    }

    public override void OnCombatantChange()
    {
        Warmode = Combatant is { Deleted: false, Alive: true } && BossPhase != 2;

        if (Warmode)
        {
            LastAggressor = Combatant;
        }
    }

    public override void Serialize(IGenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
    }

    public override void Deserialize(IGenericReader reader)
    {
        base.Deserialize(reader);
        var version = reader.ReadInt();
    }
}