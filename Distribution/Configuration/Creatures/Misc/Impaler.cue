package Misc

Impaler: {
	Name:                 "an Impaler"
	CorpseNameOverride:   "corpse of an Impaler"
	Str:                  1200
	Int:                  90
	Dex:                  300
	ActiveSpeed:          0.15
	PassiveSpeed:         0.2
	AlwaysMurderer:       true
    AutoDispel:           true
	BaseSoundID:          679
	Body:                 306
	VirtualArmor:         75
	FightMode:            "Closest"
	HitsMaxSeed:          1200
	Hue:                  0
	LootItemChance:       25
	LootItemLevel:        4
	LootTable:            "37"
	ManaMaxSeed:          80
	ProvokeSkillOverride: 130
	PhysicalImmune:       true
	StamMaxSeed:          300
	Resistances: {
		Fire:          75
        Water:         75
        Air:           75
        Poison:        6
        Earth:         100
        MagicImmunity: 7
	}
	Skills: {
		Parry:        70
		MagicResist:  110
		Tactics:      100
		Wrestling:    150
		DetectHidden: 130
	}
	Attack: {
		Speed: 60
		Damage: {
			Min: 33
			Max: 73
		}
		Ability:       "TriElementalStrike"
		AbilityChance: 1
		HitSound: 681
	}
}
