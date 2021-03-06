using Server.Spells.First;

namespace Server.Items
{
    public class MagicArrowWand : BaseWand
    {
        [Constructible]
        public MagicArrowWand() : base(WandEffect.MagicArrow, 5, 30)
        {
        }

        [Constructible]
        public MagicArrowWand(Serial serial) : base(serial)
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

        public override void OnWandUse(Mobile from)
        {
            Cast(new MagicArrowSpell(from, this));
        }
    }
}