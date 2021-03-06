namespace Server.Items
{
    [FlipableAttribute( 0xF61, 0xF60 )]
	public class Longsword : BaseSword
	{
		public override int DefaultStrengthReq{ get{ return 25; } }
		public override int DefaultMinDamage{ get{ return 5; } }
		public override int DefaultMaxDamage{ get{ return 33; } }
		public override int DefaultSpeed{ get{ return 35; } }

		public override int DefaultHitSound{ get{ return 0x237; } }
		public override int DefaultMissSound{ get{ return 0x23A; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }


		[Constructible]
public Longsword() : base( 0xF61 )
		{
			Weight = 7.0;
		}

		[Constructible]
public Longsword( Serial serial ) : base( serial )
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
