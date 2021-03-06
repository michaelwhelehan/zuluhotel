// Generated File. DO NOT MODIFY BY HAND.

namespace Server.Items
{
  [FlipableAttribute(0x1BF2, 0x1BEF)]
  public class DarkSableRubyIngot : BaseIngot
  {
    [Constructible]
    public DarkSableRubyIngot() : this(1)
    {
    }


    [Constructible]
    public DarkSableRubyIngot(int amount) : base(CraftResource.DarkSableRuby, amount)
    {
      this.Hue = 2761;
    }

    [Constructible]
    public DarkSableRubyIngot(Serial serial) : base(serial)
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
