namespace Server.Items
{
    [FlipableAttribute( 0x13d5, 0x13dd )]
	public class RangerGloves : BaseArmor
	{
		public override int InitMinHits{ get{ return 35; } }
		public override int InitMaxHits{ get{ return 45; } }

		public override int DefaultStrReq{ get{ return 25; } }

		public override int ArmorBase{ get{ return 16; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Studded; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		public override int LabelNumber{ get{ return 1041494; } } // studded gloves, ranger armor


		[Constructible]
public RangerGloves() : base( 0x13D5 )
		{
			Weight = 1.0;
			Hue = 0x59C;
		}

		[Constructible]
public RangerGloves( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( IGenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize(IGenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}