using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Sixth
{
    public class InvisibilitySpell : MagerySpell
    {
        private static readonly Hashtable m_Table = new Hashtable();

        public InvisibilitySpell(Mobile caster, Item scroll) : base(caster, scroll)
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
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (m is BaseVendor || m is PlayerVendor || m.AccessLevel > Caster.AccessLevel)
            {
                Caster.SendLocalizedMessage(501857); // This spell won't work on that!
            }
            else if (CheckBSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                Effects.SendLocationParticles(
                    EffectItem.Create(new Point3D(m.X, m.Y, m.Z + 16), Caster.Map, EffectItem.DefaultDuration), 0x376A,
                    10, 15, 5045);
                m.PlaySound(0x3C4);

                m.Hidden = true;
                m.Combatant = null;
                m.Warmode = false;

                RemoveTimer(m);

                var duration = TimeSpan.FromSeconds(1.2 * Caster.Skills.Magery.Fixed / 10);

                Timer t = new InternalTimer(m, duration);

                m_Table[m] = t;

                t.Start();
            }

            FinishSequence();
        }

        public static bool HasTimer(Mobile m)
        {
            return m_Table[m] != null;
        }

        public static void RemoveTimer(Mobile m)
        {
            var t = (Timer) m_Table[m];

            if (t != null)
            {
                t.Stop();
                m_Table.Remove(m);
            }
        }

        private class InternalTimer : Timer
        {
            private readonly Mobile m_Mobile;

            public InternalTimer(Mobile m, TimeSpan duration) : base(duration)
            {
                Priority = TimerPriority.OneSecond;
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                m_Mobile.RevealingAction();
                RemoveTimer(m_Mobile);
            }
        }

        public class InternalTarget : Target
        {
            private readonly InvisibilitySpell m_Owner;

            public InternalTarget(InvisibilitySpell owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile) m_Owner.Target((Mobile) o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}