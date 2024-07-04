using System.Threading.Tasks;
using Server;
using Server.Spells;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class InvocationSpell : ForgottenSpell, IAsyncSpell
    {
        public InvocationSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task CastAsync()
        {
            if (Utility.Random(10) < 4)
            {
                SpellHelper.Summon("GreatWyrm", Caster, 0x217);
            }
            else
            {
                SpellHelper.Summon("Drake", Caster, 0x217);
                SpellHelper.Summon("Dragon", Caster, 0x217);
            }
        }
    }
}