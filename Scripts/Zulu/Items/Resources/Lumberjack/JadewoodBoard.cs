// Generated File. DO NOT MODIFY BY HAND.
namespace Server.Items
{

    public class JadewoodBoard : Board
    {
        [Constructable]
        public JadewoodBoard() : this(1) { }

        [Constructable]
        public JadewoodBoard(int amount) : base(CraftResource.Jadewood, amount)
        {
            this.Hue = 1162;
        }

        public JadewoodBoard(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}