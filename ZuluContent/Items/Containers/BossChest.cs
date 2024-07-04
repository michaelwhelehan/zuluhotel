namespace Server.Items;

[DynamicFliping]
[Flipable(0x2811, 0x2812)]
public class CountDraculaChest : BaseBossChest
{
    public override string DefaultName => "Count Dracula's Belongings";

    [Constructible]
    public CountDraculaChest() : base(0x2812)
    {
    }

    public CountDraculaChest(Serial serial) : base(serial)
    {
    }

    public override void Serialize(IGenericWriter writer)
    {
        base.Serialize(writer);

        writer.Write(1); // version
    }

    public override void Deserialize(IGenericReader reader)
    {
        base.Deserialize(reader);

        var version = reader.ReadInt();

        if (version == 0 && Weight == 25)
            Weight = -1;
    }
}