using System;
using Server.Regions;
using Server.Spells;

namespace Server.Items
{
    public class MushroomTrap : BaseTrap
    {
        [Constructible]
        public MushroomTrap() : base(0x1125)
        {
        }

        public override bool PassivelyTriggered
        {
            get { return true; }
        }

        public override TimeSpan PassiveTriggerDelay
        {
            get { return TimeSpan.Zero; }
        }

        public override int PassiveTriggerRange
        {
            get { return 2; }
        }

        public override TimeSpan ResetDelay
        {
            get { return TimeSpan.Zero; }
        }

        public override void OnTrigger(Mobile from)
        {
            if (!from.Alive || ItemID != 0x1125 || from.AccessLevel > AccessLevel.Player)
                return;

            ItemID = 0x1126;
            Effects.PlaySound(Location, Map, 0x306);

            SpellHelper.Damage(Utility.Dice(2, 4, 0), from, from, null, TimeSpan.FromSeconds(0.5));

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), OnMushroomReset);
        }

        public virtual void OnMushroomReset()
        {
            if (Region.Find(Location, Map).IsPartOf<DungeonRegion>())
                ItemID = 0x1125; // reset
            else
                Delete();
        }

        [Constructible]
        public MushroomTrap(Serial serial) : base(serial)
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

            if (ItemID == 0x1126)
                OnMushroomReset();
        }
    }
}