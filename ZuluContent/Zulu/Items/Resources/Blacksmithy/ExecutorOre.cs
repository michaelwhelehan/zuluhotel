// Generated File. DO NOT MODIFY BY HAND.

namespace Server.Items
{
  public class ExecutorOre : BaseOre
  {
    [Constructible]
    public ExecutorOre() : this(1)
    {
    }


    [Constructible]
    public ExecutorOre(int amount) : base(CraftResource.Executor, amount)
    {
      this.Hue = 2766;
    }

    [Constructible]
    public ExecutorOre(Serial serial) : base(serial)
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
      return new ExecutorIngot();
    }
  }
}
