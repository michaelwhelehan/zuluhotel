namespace Server.Items
{
  [FlipableAttribute(0x1081, 0x1082)]
  public class WolfLeather : BaseLeather
  {
    [Constructible]
    public WolfLeather() : this(1)
    {
    }


    [Constructible]
    public WolfLeather(int amount) : base(CraftResource.WolfLeather, amount)
    {
      this.Hue = 1102;
    }

    [Constructible]
    public WolfLeather(Serial serial) : base(serial)
    {
    }

    public override void Serialize(IGenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write((int) 0); // version
    }

    public override void Deserialize(IGenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();
    }
  }
}
