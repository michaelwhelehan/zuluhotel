package Animal

SpiderLord: {
	Name:               "<random> the Spider Lord"
	CorpseNameOverride: "corpse of <random> the Spider Lord"
	Str:                1500
	Int:                2000
	Dex:                300
	ActiveSpeed:        0.15
    PassiveSpeed:       0.2
	AlwaysMurderer:     true
	BaseSoundID:        1170
	Body:               173
	CreatureType:       "Animal"
	VirtualArmor:       75
	FightMode:          "Closest"
	HitsMaxSeed:        1500
	Hue:                0
	LootItemChance:     90
    LootItemLevel:      8
    LootTable:          "150"
	ManaMaxSeed:        2000
	StamMaxSeed:        300
	Resistances: {
		Fire:          75
        Water:         75
        Air:           100
        Poison:        6
        Earth:         100
        Necro:         100
        MagicImmunity: 7
	}
	Skills: {
		Parry:        200
        MagicResist:  300
        Tactics:      200
        Wrestling:    150
        Magery:       300
        EvalInt:      200
        DetectHidden: 200
	}
	Attack: {
		Speed: 55
		Damage: {
			Min: 14
			Max: 140
		}
		HitSound:  1172
		MissSound: 1173
		HasWebs:   true
		HitPoison: "Lethal"
	}
	Equipment: [{
		ItemType:    "HeaterShield"
		Name:        "Shield AR50"
		ArmorRating: 50
	}]
}
