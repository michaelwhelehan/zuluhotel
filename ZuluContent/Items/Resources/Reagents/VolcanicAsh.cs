using System;

namespace Server.Items
{
    public class VolcanicAsh : BaseReagent
    {

        [Constructible]
public VolcanicAsh()
            : this(1)
        {
        }


        [Constructible]
public VolcanicAsh(int amount)
            : base(0xF8F, amount)
        {
        }

        [Constructible]
public VolcanicAsh(Serial serial)
            : base(serial)
        {
        }

        public override double DefaultWeight
        {
            get { return 0.1; }
        }
        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}