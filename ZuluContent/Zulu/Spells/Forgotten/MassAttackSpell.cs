using System;
using System.Threading.Tasks;
using Scripts.Zulu.Utilities;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using ZuluContent.Zulu.Engines.Magic;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class MassAttackSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        public MassAttackSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            SpellHelper.Turn(Caster, target);
            
            target.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head);
            Caster.PlaySound(0x263);
            
            var damage = SpellHelper.CalcSpellDamage(Caster, target, this);
            SpellHelper.Damage(damage, target, Caster, this);
            
            var range = (int) (Caster.Skills[CastSkill].Value / 10.0);
            var victimTarget = new AsyncTarget<Mobile>(Caster,
                new TargetOptions {Range = range});
            Caster.Target = victimTarget;
            
            Caster.RevealingAction();
            
            var (victimTargeted, _) = await victimTarget;
            
            if (victimTargeted == null)
            {
                Caster.SendFailureMessage(501589); // You can't incite that!
                return;
            }
            
            if (victimTargeted is BaseCreature { Unprovokable: true })
            {
                Caster.SendFailureMessage(1049446); // You have no chance of provoking those creatures.
                return;
            }
            
            if (!Caster.CanBeHarmful(victimTargeted, false))
            {
                Caster.SendFailureMessage(1049446); // You have no chance of provoking those creatures.
                return;
            }

            if (target == victimTargeted)
            {
                Caster.SendFailureMessage(501593); // You can't tell someone to attack themselves!
                return;
            }

            if (target.Map != victimTargeted.Map || !target.InRange(victimTargeted, range))
            {
                Caster.SendFailureMessage(1049450); // The creatures you are trying to provoke are too far away from each other for your music to have an effect.
                return;
            }
            
            if (victimTargeted is BaseCreature creatureVictim)
                creatureVictim.Provoke(Caster, target, true);
        }
    }
}