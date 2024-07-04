package Giantkin

EarthGiant: {
	Name:               "<random> the Earth Giant"
	CorpseNameOverride: "corpse of <random> the Earth Giant"
	Str:                1500
	Int:                1600
	Dex:                150
	AiType:             "AI_Mage"
	AlwaysMurderer:     true
	AutoDispel:         true
	BaseSoundID:        367
	Body:               18
	ClassLevel:         3
	ClassType:          "Mage"
	CreatureType:       "Giantkin"
	VirtualArmor:       75
	FightMode:          "Closest"
	HitsMaxSeed:        1500
	Hue:                1160
	LootItemChance:     80
	LootItemLevel:      6
	LootTable:          "9"
	ManaMaxSeed:        1600
	StamMaxSeed:        70
	PreferredSpells: ["ThorsHammer", "WindOfAmnesia", "FireSerpent", "ChaoticFlux"]
	Resistances: {
		Fire:   100
		Water:  75
		Air:    100
		Poison: 6
		Necro:  75
		Earth:  100
	}
	Skills: {
		MagicResist:  150
		Tactics:      200
		Wrestling:    150
		Magery:       200
		EvalInt:      200
		Parry:        130
		DetectHidden: 200
	}
	Attack: {
		HitSound: 362
		Speed: 65
		Damage: {
			Min: 25
			Max: 75
		}
	}
	Equipment: [{
		ItemType:    "HeaterShield"
		Name:        "Shield AR50"
		ArmorRating: 50
	}]
}
