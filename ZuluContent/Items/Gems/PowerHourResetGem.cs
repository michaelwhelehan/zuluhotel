using System;
using Scripts.Zulu.Utilities;
using Server.Mobiles;

namespace Server.Items;

public class PowerHourResetGem : Item
{
    public override double DefaultWeight => 0.1;

    public override string DefaultName => "Power Hour Reset Gem";


    [Constructible]
    public PowerHourResetGem() : base(0x103F)
    {
        Hue = 1266;
    }

    [Constructible]
    public PowerHourResetGem(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (from is PlayerMobile player)
        {
            player.LastPowerHourUsed = Core.Now - TimeSpan.FromDays(1);
            player.SendSuccessMessage("Your power hour has been reset.");
            Delete();
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