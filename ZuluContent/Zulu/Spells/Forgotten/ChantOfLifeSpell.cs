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
    public class ChantOfLifeSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        public ChantOfLifeSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            
            SpellHelper.Turn(Caster, target);
            
            var range = Caster.Skills[SkillName.Magery].Value / 10.0;
            var healed = (double) Utility.Dice(6, 8, 30);
            
            var mobiles = target.GetMobilesInRange((int) range);

            foreach (var mobile in mobiles)
            {
                if (mobile is not PlayerMobile)
                {
                    continue;
                }
                
                SpellHelper.Heal((int) healed, mobile, Caster, this);
                mobile.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                mobile.PlaySound(0x202);
            }

            mobiles.Free();
        }
    }
}