using System.Threading.Tasks;
using Server;
using Server.Spells;
using Server.Targeting;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class ThorsHammerSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        public ThorsHammerSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            SpellHelper.Turn(Caster, target);
            
            Caster.MovingEffect(target, 0x0Fb5, 4, 0, false, false);
            Caster.PlaySound(0x221);
            
            var damage = SpellHelper.CalcSpellDamage(Caster, target, this);
            SpellHelper.Damage(damage, target, Caster, this);
        }
    }
}