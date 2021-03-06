using MessagePack;
using Server;
using Server.Engines.Magic;
using ZuluContent.Zulu.Engines.Magic.Enums;

namespace ZuluContent.Zulu.Engines.Magic.Enchantments
{
    [MessagePackObject]
    public class NecroProtection : Enchantment<NecroProtectionInfo>
    {
        [IgnoreMember]
        public override string AffixName =>
            EnchantmentInfo.GetName(IElementalResistible.GetProtectionLevelForResist(Value), Cursed);

        [Key(1)]
        public int Value { get; set; } = 0;
        
        public override void OnSpellDamage(Mobile attacker, Mobile defender, ElementalType damageType, ref int damage)
        {
            if (damageType == ElementalType.Necro) 
                damage -= (int) (damage * ((double) Value / 100));
        }
    }
    
    public class NecroProtectionInfo : EnchantmentInfo
    {
        public override string Description { get; protected set; } = "Elemental Necro Protection";
        public override EnchantNameType Place { get; protected set; } = EnchantNameType.Suffix;

        public override string[,] Names { get; protected set; } = {
            {string.Empty, string.Empty},
            {"Mystic Barrier", "Evil Influence"},
            {"Divine Shielding", "Liche's Laughter"},
            {"Heavenly Sanctuary", "Daemonic Temptation"},
            {"Angelic Protection", "Dark Channeling"},
            {"Arch-Angel's Guidance", "Shadow's Touch"},
            {"Seraphim's Warding", "Guardian's Blessing"},
        };

        public override int Hue { get; protected set; } = 1170;
        public override int CursedHue { get; protected set; } = 1170;
    }
}