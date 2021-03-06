using MessagePack;
using Server;
using ZuluContent.Zulu.Engines.Magic.Enums;

namespace ZuluContent.Zulu.Engines.Magic.Enchantments
{
    [MessagePackObject]
    public class HealingBonus : Enchantment<HealingBonusInfo>
    {
        [IgnoreMember] 
        public override string AffixName => EnchantmentInfo.GetName(Value);
        [Key(1)] 
        public int Value { get; set; } = 0;
        
    }
    public class HealingBonusInfo : EnchantmentInfo
    {

        public override string Description { get; protected set; } = "Healing Bonus";
        public override EnchantNameType Place { get; protected set; } = EnchantNameType.Suffix;
        public override int Hue { get; protected set; } = 1182;
        public override int CursedHue { get; protected set; } = 0;

        public override string[,] Names { get; protected set; } = {
            {string.Empty, string.Empty},
            {"Relief", "Wounds"},
            {"Respite", "Bruises"},
            {"Rest", "Deterioration"},
            {"Regeneration", "Festering"},
            {"Healing", "Atrophy"},
            {"Nature's Blessing", "Blight"}
        };
    }
}