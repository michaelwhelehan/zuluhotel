namespace Server.Items
{
    public class CurePotion : BaseCurePotion
	{
		private static CureLevelInfo[] m_DefaultLevelInfo = new[]
			{
				new CureLevelInfo( Poison.Lesser,  1.00 ), // 100% chance to cure lesser poison
				new CureLevelInfo( Poison.Regular, 0.75 ), //  75% chance to cure regular poison
				new CureLevelInfo( Poison.Greater, 0.50 ), //  50% chance to cure greater poison
				new CureLevelInfo( Poison.Deadly,  0.15 )  //  15% chance to cure deadly poison
			};

		private static CureLevelInfo[] m_AosLevelInfo = new[]
			{
				new CureLevelInfo( Poison.Lesser,  1.00 ),
				new CureLevelInfo( Poison.Regular, 0.95 ),
				new CureLevelInfo( Poison.Greater, 0.75 ),
				new CureLevelInfo( Poison.Deadly,  0.50 ),
				new CureLevelInfo( Poison.Lethal,  0.25 )
			};

		public override CureLevelInfo[] LevelInfo{ get{ return m_DefaultLevelInfo; } }


		[Constructible]
public CurePotion() : base( PotionEffect.Cure )
		{
		}

		[Constructible]
public CurePotion( Serial serial ) : base( serial )
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
