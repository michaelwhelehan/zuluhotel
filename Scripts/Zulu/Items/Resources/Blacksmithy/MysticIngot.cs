// Generated File. DO NOT MODIFY BY HAND.
namespace Server.Items
{

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class MysticIngot : BaseIngot
    {
        [Constructable]
        public MysticIngot() : this(1) { }

        [Constructable]
        public MysticIngot(int amount) : base(CraftResource.Mystic, amount)
        {
            this.Hue = 0x17f;
        }

        public MysticIngot(Serial serial) : base(serial) { }

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