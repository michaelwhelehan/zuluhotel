using MessagePack;
using Server.Items;
using ZuluContent.Zulu.Engines.Magic.Enums;

namespace ZuluContent.Zulu.Engines.Magic.Enchantments
{

    [MessagePackObject]
    public class MeditationAllowance : Enchantment<ArmorBonusInfo>
    {
        [IgnoreMember]
        public override string AffixName => EnchantmentInfo.GetName(Value, Cursed);
        [Key(1)]
        public ArmorMeditationAllowance Value { get; set; } = ArmorMeditationAllowance.None;
    }
    
    public class AMeditationAllowanceInfo : EnchantmentInfo
    {
        public override string Description { get; protected set; } = "Meditation Allowance";
        public override EnchantNameType Place { get; protected set; } = EnchantNameType.Prefix;
        public override int Hue { get; protected set; } = 1109;
        public override int CursedHue { get; protected set; } = 0;

        public override string[,] Names { get; protected set; } = {
            {string.Empty, string.Empty}, // None
            {string.Empty, string.Empty},
            {string.Empty, string.Empty},
            {string.Empty, string.Empty},
            {string.Empty, string.Empty},
            {string.Empty, string.Empty},
            {string.Empty, string.Empty},
        };
    }
}