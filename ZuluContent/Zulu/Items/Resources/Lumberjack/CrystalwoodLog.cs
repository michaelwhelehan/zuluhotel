// Generated File. DO NOT MODIFY BY HAND.

namespace Server.Items
{
  public class CrystalwoodLog : Log
  {
    [Constructible]
    public CrystalwoodLog() : this(1)
    {
    }


    [Constructible]
    public CrystalwoodLog(int amount) : base(CraftResource.Crystalwood, amount)
    {
      this.Hue = 2759;
    }

    [Constructible]
    public CrystalwoodLog(Serial serial) : base(serial)
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
      if (!TryCreateBoards(from, 125, new CrystalwoodBoard()))
      {
        return false;
      }

      return true;
    }
  }
}
