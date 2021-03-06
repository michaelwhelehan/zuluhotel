namespace Server.Items
{
    [FlipableAttribute( 0x1403, 0x1402 )]
	public class ShortSpear : BaseSpear
	{
		public override int DefaultStrengthReq{ get{ return 15; } }
		public override int DefaultMinDamage{ get{ return 4; } }
		public override int DefaultMaxDamage{ get{ return 32; } }
		public override int DefaultSpeed{ get{ return 50; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		public override WeaponAnimation DefaultAnimation{ get{ return WeaponAnimation.Pierce1H; } }


		[Constructible]
public ShortSpear() : base( 0x1403 )
		{
			Weight = 4.0;
		}

		[Constructible]
public ShortSpear( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( IGenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( IGenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
