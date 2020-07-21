// Generated File. DO NOT MODIFY BY HAND.
namespace Server.Items
{

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class GoddessIngot : BaseIngot
    {
        [Constructable]
        public GoddessIngot() : this(1) { }

        [Constructable]
        public GoddessIngot(int amount) : base(CraftResource.Goddess, amount)
        {
            this.Hue = 2774;
        }

        public GoddessIngot(Serial serial) : base(serial) { }

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