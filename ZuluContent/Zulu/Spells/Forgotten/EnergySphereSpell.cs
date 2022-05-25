using System.Threading.Tasks;
using Server;
using Server.Engines.Magic;
using Server.Spells;
using Server.Targeting;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class EnergySphereSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        public EnergySphereSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            SpellHelper.Turn(Caster, target);
            
            Caster.MovingEffect(target, 0x37B9, 4, 0, false, false);
            Caster.PlaySound(0x211);
            
            var damage = SpellHelper.CalcSpellDamage(Caster, target, this);
            SpellHelper.Damage(damage, target, Caster, this);
            SpellHelper.Damage(damage, target, Caster, this, damageType: ElementalType.Earth);
        }
    }
}