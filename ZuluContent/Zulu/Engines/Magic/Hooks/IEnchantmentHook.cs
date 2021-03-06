using System;
using Server;
using Server.Engines.Magic;
using Server.Items;
using Server.Spells;

namespace ZuluContent.Zulu.Engines.Magic.Hooks
{
    public interface IEnchantmentHook
    {
        public void OnIdentified(IEntity entity);

        public void OnAdded(IEntity entity);

        public void OnRemoved(IEntity entity);

        public void OnSpellDamage(Mobile attacker, Mobile defender, ElementalType damageType, ref int damage);

        public void OnGetCastDelay(Mobile mobile, Spell spell, ref double delay);

        public void OnBeforeSwing(Mobile attacker, Mobile defender);

        public void OnSwing(Mobile attacker, Mobile defender, ref double damageBonus, ref TimeSpan result);

        public void OnGetSwingDelay(ref double delayInSeconds, Mobile m);

        public void OnCheckHit(Mobile attacker, Mobile defender, ref bool result);

        public void OnMeleeHit(Mobile attacker, Mobile defender, BaseWeapon weapon, ref int damage);

        public void OnAbsorbMeleeDamage(Mobile attacker, Mobile defender, BaseWeapon weapon, ref int damage);

        public void OnShieldHit(Mobile attacker, Mobile defender, BaseWeapon weapon, BaseShield shield, ref int damage);

        public void OnArmorHit(Mobile attacker, Mobile defender, BaseWeapon weapon, BaseArmor armor, ref int damage);
    }
}