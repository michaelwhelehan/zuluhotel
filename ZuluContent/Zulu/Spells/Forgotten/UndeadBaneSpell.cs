using System.Threading.Tasks;
using Server;
using Server.Spells;
using Server.Targeting;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class UndeadBaneSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        public UndeadBaneSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            SpellHelper.Turn(Caster, target);
            
            target.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head);
            target.PlaySound(0x24E);
            target.PlaySound(0x214);
            
            var damage = SpellHelper.CalcSpellDamage(Caster, target, this);
            SpellHelper.Damage(damage, target, Caster, this);
            SpellHelper.Damage(damage, target, Caster, this);
            SpellHelper.Damage(damage / 2, target, Caster, this);
        }
    }
}