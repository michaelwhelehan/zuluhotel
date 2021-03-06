using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Scripts.Zulu.Spells.Necromancy
{
    public class DecayingRaySpell : NecromancerSpell
    {
        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(1.0); }
        }

        public override double RequiredSkill
        {
            get { return 80.0; }
        }

        public override int RequiredMana
        {
            get { return 40; }
        }

        public DecayingRaySpell(Mobile caster, Item scroll) : base(caster, scroll)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                // Seems like this should be responsibility of the targetting system.  --daleron
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
                goto Return;
            }

            if (!CheckSequence()) goto Return;

            // Prevent stacking of DecayingRay.
            if (!m.BeginAction(typeof(DecayingRaySpell))) goto Return;

            // Turn caster towards the target.
            SpellHelper.Turn(Caster, m);

            // TODO: Spell Effects.

            m.PlaySound(0x0fe);
            Caster.DoHarmful(m);

            // Feel free to fck with this formula, I just mostly copied it from POL 093.  I don't care so long as it's balanced --sith
            var val = Caster.Skills[SkillName.SpiritSpeak].Value;
            val /= 10;
            val += 5;
            val *= 2;

            if (val < 0)
                val = 0;
            else if (val > 75)
                val = 75;

            m.VirtualArmorMod -= (int) val;

            new InternalTimer(m, Caster, (int) val).Start();

            Return:
            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Target;
            private int m_Value;

            public InternalTimer(Mobile target, Mobile caster, int value) : base(TimeSpan.FromSeconds(0))
            {
                m_Value = value;
                m_Target = target;

                // TODO: Compute a reasonable duration, this is stolen from ArchProtection
                // durations are done in POL under scripts/include/dotempmods.inc under GetModDuration or somesuch --sith
                var time = caster.Skills[SkillName.Magery].Value * 1.2;
                if (time > 144)
                    time = 144;
                Delay = TimeSpan.FromSeconds(time);
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_Target.EndAction(typeof(DecayingRaySpell));
                m_Target.VirtualArmorMod += m_Value;
            }
        }

        private class InternalTarget : Target
        {
            private DecayingRaySpell m_Owner;

            // TODO: What is thie Core.ML stuff, is it needed?
            public InternalTarget(DecayingRaySpell owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile) o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}