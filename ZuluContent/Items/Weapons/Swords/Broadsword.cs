namespace Server.Items
{
    [FlipableAttribute( 0xF5E, 0xF5F )]
	public class Broadsword : BaseSword
	{
		public override int DefaultStrengthReq{ get{ return 25; } }
		public override int DefaultMinDamage{ get{ return 5; } }
		public override int DefaultMaxDamage{ get{ return 29; } }
		public override int DefaultSpeed{ get{ return 45; } }

		public override int DefaultHitSound{ get{ return 0x237; } }
		public override int DefaultMissSound{ get{ return 0x23A; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 100; } }


		[Constructible]
public Broadsword() : base( 0xF5E )
		{
			Weight = 6.0;
		}

		[Constructible]
public Broadsword( Serial serial ) : base( serial )
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
