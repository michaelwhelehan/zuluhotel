using System;
using MessagePack;
using Server;
using Server.Spells;
using ZuluContent.Zulu.Engines.Magic.Enums;

namespace ZuluContent.Zulu.Engines.Magic.Enchantments.Buffs
{
    [MessagePackObject]
    public class FireSerpent : Enchantment<FireSerpent.FireSerpentInfo>, IBuff
    {
        [IgnoreMember] public override string AffixName => string.Empty;

        [Key(1)]
        public int Value
        {
            get;
            set;
        }
        
        public override void OnResurrect(Mobile patient)
        {
            patient.Poison = Poison.GetPoison(Value);
            
            if (patient is IBuffable buffable)
                buffable.BuffManager.RemoveBuff(this);
        }

        #region IBuff
        [IgnoreMember] public BuffIcon Icon { get; init; } = BuffIcon.PainSpike;
        [IgnoreMember] public string Title { get; init; } = "Fire Serpent";
        [IgnoreMember] public string Description { get; init; } = "Poison Upon Resurrection";
        [IgnoreMember] public string[] Details { get; init; }
        [IgnoreMember] public bool ExpireOnDeath { get; init; } = false;
        [IgnoreMember] public bool Dispellable { get; init; } = true;
        [IgnoreMember] public TimeSpan Duration { get; init; }
        [IgnoreMember] public DateTime Start { get; init; }

        public void OnBuffAdded(Mobile parent)
        {
            (parent as IEnchanted)?.Enchantments.Set(this);
        }

        public void OnBuffRemoved(Mobile parent)
        {
            if (parent is IEnchanted enchanted)
                enchanted.Enchantments.Remove(this);
        }

        #endregion

        #region EnchantmentInfo

        public class FireSerpentInfo : EnchantmentInfo
        {
            public override string Description { get; protected set; } = "Fire Serpent";
            public override EnchantNameType Place { get; protected set; } = EnchantNameType.Prefix;
            public override int Hue { get; protected set; } = 0;
            public override int CursedHue { get; protected set; } = 0;
            public override string[,] Names { get; protected set; } = { };
            
            public override string GetName(int index, CurseType curse = CurseType.None)
            {
                return string.Empty;
            }
        }

        #endregion
    }
}