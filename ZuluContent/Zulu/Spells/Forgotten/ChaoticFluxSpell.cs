using System.Threading.Tasks;
using Server;
using Server.Spells;
using Server.Targeting;
using ZuluContent.Zulu.Engines.Magic.Enchantments.Buffs;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class ChaoticFluxSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        public ChaoticFluxSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            
            if (!Caster.CanBuff(target, true, BuffIcon.HitLowerDefense))
                return;
            
            SpellHelper.Turn(Caster, target);

            Caster.MovingEffect(target, 0x36FE, 4, 0, false, false);
            Caster.PlaySound(0x1DC);
            
            var modAmount = (int) (SpellHelper.GetModAmount(Caster, target, StatType.All) * 1.8) * -1;
            var duration = SpellHelper.GetDuration(Caster, target) * 1.8;
            
            target.TryAddBuff(new StatBuff(StatType.All)
            {
                Title = "Chaotic Flux",
                Icon = BuffIcon.HitLowerDefense,
                Value = modAmount,
                Duration = duration,
            });
            
            target.Spell?.OnCasterHurt();
            target.Paralyzed = false;
        }
    }
}