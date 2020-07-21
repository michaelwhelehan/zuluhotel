namespace Server.Items
{
    [FlipableAttribute( 0xE87, 0xE88 )]
	public class Pitchfork : BaseSpear
	{
		public override int DefaultStrengthReq{ get{ return 15; } }
		public override int DefaultMinDamage{ get{ return 4; } }
		public override int DefaultMaxDamage{ get{ return 16; } }
		public override int DefaultSpeed{ get{ return 45; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 60; } }


		[Constructible]
public Pitchfork() : base( 0xE87 )
		{
			Weight = 11.0;
		}

		[Constructible]
public Pitchfork( Serial serial ) : base( serial )
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

			if ( Weight == 10.0 )
				Weight = 11.0;
		}
	}
}