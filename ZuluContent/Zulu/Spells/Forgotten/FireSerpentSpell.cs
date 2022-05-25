using System.Threading.Tasks;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using ZuluContent.Zulu.Engines.Magic.Enchantments.Buffs;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class FireSerpentSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        public FireSerpentSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            SpellHelper.Turn(Caster, target);
            
            Caster.MovingEffect(target, 0x36F4, 7, 0, false, false);
            Caster.PlaySound(0x226);
            
            var damage = SpellHelper.CalcSpellDamage(Caster, target, this);

            SpellHelper.Damage(damage, target, Caster, this);
            
            target.ApplyPoison(Caster, Poison.Deadly);

            if (target is PlayerMobile player)
                player.TryAddBuff(new FireSerpent { Value = Poison.Deadly.Level });
        }
    }
}