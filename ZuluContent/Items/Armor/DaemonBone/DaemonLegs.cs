namespace Server.Items
{
    [FlipableAttribute( 0x1452, 0x1457 )]
	public class DaemonLegs : BaseArmor
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		public override int DefaultStrReq{ get{ return 40; } }

		public override int DefaultDexBonus{ get{ return -4; } }

		public override int ArmorBase{ get{ return 46; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Bone; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		public override int LabelNumber{ get{ return 1041375; } } // daemon bone leggings


		[Constructible]
public DaemonLegs() : base( 0x1452 )
		{
			Weight = 3.0;
			Hue = 0x648;
		}

		[Constructible]
public DaemonLegs( Serial serial ) : base( serial )
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
