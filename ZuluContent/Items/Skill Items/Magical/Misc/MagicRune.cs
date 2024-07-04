using System;
using ModernUO.Serialization;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class MagicRune : Item
{
    [Constructible]
    public MagicRune(int itemID, TimeSpan duration) : base(itemID)
    {
        Movable = false;
        
        Light = LightType.Circle225;

        new RuneTimer(this, duration).Start();
    }

    [AfterDeserialization]
    private void OnAfterDeserialization()
    {
        Delete();
    }

    private class RuneTimer : Timer
    {
        private MagicRune m_Rune;

        public RuneTimer(MagicRune rune, TimeSpan duration) : base(duration)
        {
            m_Rune = rune;
        }

        protected override void OnTick()
        {
            m_Rune.Delete();
        }
    }
}

[SerializationGenerator(0, false)]
public partial class MagicHealingRune : MagicRune
{
    [Constructible]
    public MagicHealingRune(Mobile caster, Spell spell, TimeSpan duration, double range) : base(0x4856, duration)
    {
        new RuneTimer(caster, spell, this, range).Start();
    }

    private class RuneTimer : Timer
    {
        private Mobile m_Caster;
        private Spell m_Spell;
        private MagicHealingRune m_Rune;
        private double m_Range;

        public RuneTimer(Mobile caster, Spell spell, MagicHealingRune rune, double range) : base(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5))
        {
            m_Caster = caster;
            m_Spell = spell;
            m_Rune = rune;
            m_Range = range;
        }

        protected override void OnTick()
        {
            var healed = (double) Utility.Dice(6, 8, 10);
            
            var mobiles = m_Rune.GetMobilesInRange((int) m_Range);

            foreach (var mobile in mobiles)
            {
                if (mobile is not PlayerMobile || !mobile.InLOS(m_Rune))
                {
                    continue;
                }
                
                SpellHelper.Heal((int) healed, mobile, m_Caster, m_Spell);
                mobile.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                mobile.PlaySound(0x5C9);
            }
            
            mobiles.Free();
        }
    }
}

[SerializationGenerator(0, false)]
public partial class MagicDamageRune : MagicRune
{
    [Constructible]
    public MagicDamageRune(Mobile caster, Spell spell, TimeSpan duration, double range) : base(0x4859, duration)
    {
        new RuneTimer(caster, spell, this, range).Start();
    }

    private class RuneTimer : Timer
    {
        private Mobile m_Caster;
        private Spell m_Spell;
        private MagicDamageRune m_Rune;
        private double m_Range;

        public RuneTimer(Mobile caster, Spell spell, MagicDamageRune rune, double range) : base(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5))
        {
            m_Caster = caster;
            m_Spell = spell;
            m_Rune = rune;
            m_Range = range;
        }

        protected override void OnTick()
        {
            var damage = (double) Utility.Dice(5, 6, 8);
            
            var mobiles = m_Rune.GetMobilesInRange((int) m_Range);

            foreach (var mobile in mobiles)
            {
                if (mobile is PlayerMobile || !mobile.InLOS(m_Rune))
                {
                    continue;
                }
                
                SpellHelper.Damage((int) damage, mobile, m_Caster, m_Spell);
                mobile.FixedParticles(0x37C4, 9, 32, 5030, EffectLayer.Waist);
                mobile.PlaySound(0x0FC);
            }
            
            mobiles.Free();
        }
    }
}