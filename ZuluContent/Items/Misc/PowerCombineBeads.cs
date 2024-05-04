using System;
using Scripts.Zulu.Utilities;
using Server.Engines.Craft;
using Server.Engines.Magic;
using Server.Multis;
using Server.Spells;
using Server.Targeting;
using ZuluContent.Zulu.Engines.Magic.Enchantments;
using ZuluContent.Zulu.Engines.Magic.Enums;
using ZuluContent.Zulu.Items;
using static ZuluContent.Zulu.Items.SingleClick.SingleClickHandler;

namespace Server.Items
{
    public class PowerCombineBeads : BaseEquippableItem
    {
        [Constructible]
        public PowerCombineBeads() : base(0x108B)
        {
        }

        public PowerCombineBeads(Serial serial) : base(serial)
        {
        }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorBonusType ArmorBonusType
        {
            get => Enchantments.Get((ArmorBonus e) => e.Value);
            set
            {
                if (value > ArmorBonusType.Adamantium)
                    return;

                Enchantments.Set((ArmorBonus e) => e.Value = value);
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            HandleSingleClick(this, from);
        }
        
        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendFailureMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (Identified == false)
            {
                from.SendFailureMessage("You must identify those first.");
                return;
            }
            
            if (from.InRange(GetWorldLocation(), 1))
            {
                from.SendSuccessMessage("Target the item to imbue with these beads");
                from.Target = new InternalTarget(this);
            }
            else
            {
                from.SendFailureMessage(500446); // That is too far away.
            }
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        private class InternalTarget : Target
        {
            private PowerCombineBeads m_Beads;

            public InternalTarget(PowerCombineBeads beads) : base(1, false, TargetFlags.None)
            {
                m_Beads = beads;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IGMItem)
                {
                    from.SendFailureMessage("Power Combine Beads may not be used on GM items.");
                    return;
                }
                
                if (targeted is BaseClothing or BaseJewel)
                {
                    var targetedItem = (BaseEquippableItem)targeted;

                    foreach (var (_, value) in m_Beads.Enchantments.Values)
                    {
                        if (value is FirstSkillBonus newSkillBonus)
                        {
                            if (newSkillBonus.Skill == targetedItem.FirstSkillBonusName)
                            {
                                targetedItem.FirstSkillBonusValue = Math.Min(targetedItem.FirstSkillBonusValue + newSkillBonus.Value, 6);
                            }
                            
                        }
                        else if (value is StrBonus newStrBonus)
                        {
                            targetedItem.StrBonus = Math.Min(targetedItem.StrBonus + newStrBonus.Value, 30);
                        }
                        else if (value is IntBonus newIntBonus)
                        {
                            targetedItem.IntBonus = Math.Min(targetedItem.IntBonus + newIntBonus.Value, 30);
                        }
                        else if (value is DexBonus newDexBonus)
                        {
                            targetedItem.DexBonus = Math.Min(targetedItem.DexBonus + newDexBonus.Value, 30);
                        }
                        
                        targetedItem.Enchantments.Set(value);
                    }

                    /*if (targetedItem.Hue == 0 && m_Beads.Hue != 0)
                        targetedItem.Hue = m_Beads.Hue;*/
                    targetedItem.Delta(ItemDelta.Update);

                    m_Beads.Delete();

                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, 14201, 16);
                    from.SendSuccessMessage("The item has been imbued with the bead's properties.");
                }
                else
                {
                    from.SendFailureMessage("Power Combine Beads may only be used on clothing or jewelry.");
                }
            }
        }
    }
}