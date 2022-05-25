using System;
using System.Collections;
using System.Threading.Tasks;
using Server;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using ZuluContent.Zulu.Engines.Magic.Enchantments.Buffs;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class ChantOfEnhanceSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        public ChantOfEnhanceSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            
            SpellHelper.Turn(Caster, target);
            
            var range = Caster.Skills[SkillName.Magery].Value / 10.0;

            var mobiles = target.GetMobilesInRange((int) range);

            foreach (var mobile in mobiles)
            {
                if (!(mobile is PlayerMobile))
                {
                    continue;
                }
                
                if (!Caster.CanBuff(mobile, true, BuffIcon.BloodOathCaster))
                    continue;
                
                var modAmount = (int) (SpellHelper.GetModAmount(Caster, Caster, StatType.All) * 1.8);
                var duration = SpellHelper.GetDuration(Caster, mobile) * 1.8;

                mobile.TryAddBuff(new StatBuff(StatType.All)
                {
                    Title = "Chant of Enhance",
                    Icon = BuffIcon.BloodOathCaster,
                    Value = modAmount,
                    Duration = duration
                });
                
                mobile.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                mobile.PlaySound(0x244);
            }

            mobiles.Free();
        }
    }
}