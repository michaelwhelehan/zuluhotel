using System;
using System.Threading.Tasks;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using ZuluContent.Zulu.Engines.Magic;
using ZuluContent.Zulu.Engines.Magic.Enchantments.Buffs;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class SpectralAuraSpell : ForgottenSpell, IAsyncSpell
    {
        public SpectralAuraSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task CastAsync()
        {
            var range = Caster.Skills[SkillName.Magery].Value / 10.0;
            var duration = Caster.Skills[SkillName.Magery].Value;
            
            Caster.FireHook(h => h.OnModifyWithMagicEfficiency(Caster, ref duration));

            var mobiles = Caster.GetMobilesInRange((int) range);

            foreach (var mobile in mobiles)
            {
                if (mobile is not PlayerMobile || !Caster.CanBuff(mobile, icons: BuffIcon.HidingAndOrStealth))
                {
                    continue;
                }
                
                mobile.TryAddBuff(new Invisibility
                {
                    Icon = BuffIcon.Invisibility,
                    Title = "Invisible",
                    Duration = TimeSpan.FromSeconds(duration),
                    Start = DateTime.UtcNow,
                });
                
                Effects.SendLocationParticles(
                    EffectItem.Create(new Point3D(mobile.X, mobile.Y, mobile.Z + 16), Caster.Map, EffectItem.DefaultDuration), 0x376A,
                    10, 15, 5045);
                mobile.PlaySound(0x3C4);
            }

            mobiles.Free();
        }
    }
}