// Generated File. DO NOT MODIFY BY HAND.

namespace Server.Items
{
  public class ZuluBoard : Board
  {
    [Constructible]
    public ZuluBoard() : this(1)
    {
    }


    [Constructible]
    public ZuluBoard(int amount) : base(CraftResource.Zulu, amount)
    {
      this.Hue = 2749;
    }

    [Constructible]
    public ZuluBoard(Serial serial) : base(serial)
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
