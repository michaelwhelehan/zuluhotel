package Undead

CountDracula: {
	Name:               "a Poor Man"
	CorpseNameOverride: "corpse of Count Dracula"
	BaseType:           "CountDracula"
	Str:                2250
	Int:                500
	Dex:                450
	ActiveSpeed:        0.1
    PassiveSpeed:       0.2
	AlwaysMurderer:     true
	ClassLevel:         5
    ClassType:          "Warrior"
	CreatureType:       "Undead"
	VirtualArmor:       35
	FightMode:          "Aggressor"
	HitsMaxSeed:        22500
	LootItemChance:     100
    LootItemLevel:      9
	LootTable:          "160"
	ManaMaxSeed:        800
	StamMaxSeed:        500
	Resistances: {
        Poison:        6
        Water:         50
        Air:           50
        Earth:         50
        Necro:         100
        MagicImmunity: 7
    }
    Skills: {
        Tactics:      250
        Swords:       150
        Magery:       150
        MagicResist:  300
        Parry:        300
        DetectHidden: 200
    }
	Attack: {
		Ability:       "LifeDrainStrike"
		AbilityChance: 1.0
		Speed:         35
		Damage: {
			Min: 16
			Max: 38
		}
		HitSound: 601
	}
	Equipment: [
		{
			ItemType: "ShortHair"
			Hue:      1
			Lootable: false
		},
		{
            ItemType: "Longsword"
            Lootable: true
        }
	]
}
