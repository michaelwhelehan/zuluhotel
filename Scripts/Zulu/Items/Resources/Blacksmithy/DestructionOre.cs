// Generated File. DO NOT MODIFY BY HAND.
namespace Server.Items
{

    public class DestructionOre : BaseOre
    {
        [Constructable]
        public DestructionOre() : this(1) { }

        [Constructable]
        public DestructionOre(int amount) : base(CraftResource.Destruction, amount)
        {
            this.Hue = 2773;
        }

        public DestructionOre(Serial serial) : base(serial) { }

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

        public override BaseIngot GetIngot()
        {
            return new DestructionIngot();
        }
    }
}