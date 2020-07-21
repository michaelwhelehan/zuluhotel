// Generated File. DO NOT MODIFY BY HAND.
namespace Server.Items
{

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class MalachiteIngot : BaseIngot
    {
        [Constructable]
        public MalachiteIngot() : this(1) { }

        [Constructable]
        public MalachiteIngot(int amount) : base(CraftResource.Malachite, amount)
        {
            this.Hue = 2748;
        }

        public MalachiteIngot(Serial serial) : base(serial) { }

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