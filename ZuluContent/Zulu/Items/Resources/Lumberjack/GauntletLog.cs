// Generated File. DO NOT MODIFY BY HAND.

namespace Server.Items
{
  public class GauntletLog : Log
  {
    [Constructible]
    public GauntletLog() : this(1)
    {
    }


    [Constructible]
    public GauntletLog(int amount) : base(CraftResource.Gauntlet, amount)
    {
      this.Hue = 2777;
    }

    [Constructible]
    public GauntletLog(Serial serial) : base(serial)
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

    public override bool Axe(Mobile from, BaseAxe axe)
    {
      if (!TryCreateBoards(from, 95, new GauntletBoard()))
      {
        return false;
      }

      return true;
    }
  }
}
