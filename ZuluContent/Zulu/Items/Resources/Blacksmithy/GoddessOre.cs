// Generated File. DO NOT MODIFY BY HAND.

namespace Server.Items
{
  public class GoddessOre : BaseOre
  {
    [Constructible]
    public GoddessOre() : this(1)
    {
    }


    [Constructible]
    public GoddessOre(int amount) : base(CraftResource.Goddess, amount)
    {
      this.Hue = 2774;
    }

    [Constructible]
    public GoddessOre(Serial serial) : base(serial)
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

    public override BaseIngot GetIngot()
    {
      return new GoddessIngot();
    }
  }
}
