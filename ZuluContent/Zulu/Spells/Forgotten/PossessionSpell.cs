using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server;
using Server.Spells;
using Server.Targeting;
using ZuluContent.Zulu.Engines.Magic.Enchantments.Buffs;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class PossessionSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        public PossessionSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            SpellHelper.Turn(Caster, target);

            if (Utility.Random(10) < 3)
            {
                if (!Caster.CanBuff(Caster, true, BuffIcon.Curse, BuffIcon.EtherealVoyage))
                    return;

                var modAmount = (int) new List<int> { target.Str, target.Int, target.Dex }.Average();
                var duration = SpellHelper.GetDuration(Caster, Caster) * 2.0;
                
                Caster.PlaySound(0x103);
                
                Caster.TryAddBuff(new StatBuff(StatType.All)
                {
                    Title = "Possession",
                    Icon = BuffIcon.EtherealVoyage,
                    Value = modAmount,
                    Duration = duration
                });
            }
            else
            {
                Caster.Kill();
            }
        }
    }
}