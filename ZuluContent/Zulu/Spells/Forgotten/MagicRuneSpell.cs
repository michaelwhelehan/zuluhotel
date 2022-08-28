using System;
using System.Threading.Tasks;
using Scripts.Zulu.Utilities;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class MagicRuneSpell : ForgottenSpell, ITargetableAsyncSpell<IPoint3D>
    {
        public MagicRuneSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<IPoint3D> response)
        {
            if (!response.HasValue)
                return;

            var point = SpellHelper.GetSurfaceTop(response.Target);
            
            var map = Caster.Map;
            
            if (map == null || !Caster.InLOS(point))
            {
                Caster.SendFailureMessage(501942); // That location is blocked.
                return;
            }

            var runeId = await new MagicRuneGump(Caster);
            
            if (runeId <= 0)
                return;
            
            var range = Caster.Skills[SkillName.Magery].Value / 25.0;

            var magicRuneFound = false;
            
            IPooledEnumerable eable = map.GetItemsInRange(point, (int)(range * 2));

            foreach (Item item in eable)
                if (runeId == 1 && item is MagicHealingRune || runeId == 2 && item is MagicDamageRune)
                {
                    magicRuneFound = true;
                    break;
                }

            eable.Free();

            if (magicRuneFound)
            {
                Caster.SendFailureMessage("You cannot place two of the same runes near each other.");
                return;
            }

            var duration = TimeSpan.FromSeconds(Utility.RandomMinMax(40, 80));

            MagicRune magicRune = runeId switch
            {
                1 => new MagicHealingRune(Caster, this, duration, range),
                2 => new MagicDamageRune(Caster, this, duration, range),
                _ => new MagicHealingRune(Caster, this, duration, range)
            };
            magicRune.MoveToWorld(point, map);
            Caster.PlaySound(0x0FA);
        }
    }
}