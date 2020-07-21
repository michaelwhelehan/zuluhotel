namespace Server.Items
{
    [FlipableAttribute( 0xF62, 0xF63 )]
	public class Spear : BaseSpear
	{
		public override int DefaultStrengthReq{ get{ return 30; } }
		public override int DefaultMinDamage{ get{ return 2; } }
		public override int DefaultMaxDamage{ get{ return 36; } }
		public override int DefaultSpeed{ get{ return 46; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }


		[Constructible]
public Spear() : base( 0xF62 )
		{
			Weight = 7.0;
		}

		[Constructible]
public Spear( Serial serial ) : base( serial )
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