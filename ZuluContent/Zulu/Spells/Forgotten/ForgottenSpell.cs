using Server;
using Server.Spells;

namespace Scripts.Zulu.Spells.Forgotten
{
    public abstract class ForgottenSpell : Spell
    {
        public ForgottenSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }
    }
}