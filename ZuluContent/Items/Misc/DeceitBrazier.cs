using System;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class DeceitBrazier : Item
	{
		private static Type[] m_Creatures = new[]
			{
				#region Undead
				typeof( Skeleton ), 		 		 		typeof( Mummy ),
				typeof( BoneKnight ), 		typeof( Liche ), 				typeof( LicheLord ),
				typeof( Wraith ), 			typeof( Shade ), 				typeof( Spectre ), 				typeof( Zombie ),

				#endregion

				#region Demons
				typeof( Balron ), 			typeof( Daemon ),				typeof( Imp ),
				typeof( Mongbat ), 			typeof( IceFiend ), 			typeof( Gargoyle ), 			typeof( StoneGargoyle ),
				#endregion

				#region Gazers
				typeof( Gazer ),
				#endregion

				#region Uncategorized
				typeof( Harpy ),						typeof( HeadlessOne ),			typeof( HellHound ),
				typeof( HellCat ),			typeof( Phoenix ),				typeof( LavaLizard ),
						typeof( PredatorHellCat ),		typeof( Wisp ),
				#endregion

				#region Arachnid
				typeof( PhaseSpider ), 		typeof( GiantFrostSpider ), 			typeof( GiantScorpion ),
				#endregion

				#region Repond
				typeof( Cyclops ), 			typeof( Ettin ),                typeof( EvilMage ),
				typeof( TrollLord ),		typeof( OgreLord ), 			typeof( OrcCaptain ),
				typeof( OrcishLord ), 		typeof( OrcMasterMage ), 			typeof( Ratlord ),
				typeof( RatmanMarksman ),		typeof( OrcCaptain ),			typeof( Troll ),				typeof( Titan ),
				typeof( EvilMage ),
				#endregion

				#region Reptilian
				typeof( Dragon ), 			typeof( Drake ), 				typeof( Snake ),
				typeof( IceSerpent ), 		typeof( GiantSerpent ), 		typeof( IceSnake ), 			typeof( LavaSerpent ),
				typeof( LizardmanKing ), 		typeof( Wyvern ),				typeof( PoisonWyrm ),
				typeof( SilverSerpent ), 	typeof( LavaSnake ),
				#endregion

				#region Elementals
				typeof( EarthElemental ), 	typeof( PoisonElemental ),		typeof( FireElemental ),
				typeof( IceElemental ),		typeof( WaterElemental ),
				typeof( AirElemental ),
				#endregion

				#region Random Critters
				typeof( Sewerrat ),			typeof( GiantRat ), 			typeof( DireWolf ),				typeof( TimberWolf ),
				typeof( Cougar ), 			typeof( Alligator )
				#endregion
			};

		public static Type[] Creatures { get { return m_Creatures; } }

		private Timer m_Timer;
		private DateTime m_NextSpawn;
		private int m_SpawnRange;
		private TimeSpan m_NextSpawnDelay;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextSpawn { get { return m_NextSpawn; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SpawnRange { get { return m_SpawnRange; } set { m_SpawnRange = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextSpawnDelay { get { return m_NextSpawnDelay; } set { m_NextSpawnDelay = value; } }

		public override int LabelNumber { get { return 1023633; } } // Brazier


		[Constructible]
public DeceitBrazier() : base( 0xE31 )
		{
			Movable = false;
			Light = LightType.Circle225;
			m_NextSpawn = DateTime.Now;
			m_NextSpawnDelay = TimeSpan.FromMinutes( 15.0 );
			m_SpawnRange = 5;
		}

		[Constructible]
public DeceitBrazier( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( IGenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version

			writer.Write( (int)m_SpawnRange );
			writer.Write( m_NextSpawnDelay );
		}

		public override void Deserialize( IGenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( version >= 0 )
			{
				m_SpawnRange = reader.ReadInt();
				m_NextSpawnDelay = reader.ReadTimeSpan();
			}

			m_NextSpawn = DateTime.Now;
		}

		public virtual void HeedWarning()
		{
			PublicOverheadMessage( MessageType.Regular, 0x3B2, 500761 );// Heed this warning well, and use this brazier at your own peril.
		}

		public override bool HandlesOnMovement { get { return true; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if( m_NextSpawn < DateTime.Now ) // means we haven't spawned anything if the next spawn is below
			{
				if( Utility.InRange( m.Location, Location, 1 ) && !Utility.InRange( oldLocation, Location, 1 ) && m.Player && !(m.AccessLevel > AccessLevel.Player || m.Hidden) )
				{
					if( m_Timer == null || !m_Timer.Running )
						m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( 2 ), HeedWarning );
				}
			}

			base.OnMovement( m, oldLocation );
		}

		public Point3D GetSpawnPosition()
		{
			Map map = Map;

			if( map == null )
				return Location;

			// Try 10 times to find a Spawnable location.
			for( int i = 0; i < 10; i++ )
			{
				int x = Location.X + (Utility.Random( m_SpawnRange * 2 + 1 ) - m_SpawnRange);
				int y = Location.Y + (Utility.Random( m_SpawnRange * 2 + 1 ) - m_SpawnRange);
				int z = Map.GetAverageZ( x, y );

				if( Map.CanSpawnMobile( new Point2D( x, y ), Z ) )
					return new Point3D( x, y, Z );
				else if( Map.CanSpawnMobile( new Point2D( x, y ), z ) )
					return new Point3D( x, y, z );
			}

			return Location;
		}

		public virtual void DoEffect( Point3D loc, Map map )
		{
			Effects.SendLocationParticles( EffectItem.Create( loc, map, EffectItem.DefaultDuration ), 0x3709, 10, 30, 5052 );
			Effects.PlaySound( loc, map, 0x225 );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( Utility.InRange( from.Location, Location, 2 ) )
			{
				try
				{
					if( m_NextSpawn < DateTime.Now )
					{
						Map map = Map;
						BaseCreature bc = (BaseCreature)Activator.CreateInstance( m_Creatures[Utility.Random( m_Creatures.Length )] );

						if( bc != null )
						{
							Point3D spawnLoc = GetSpawnPosition();

							DoEffect( spawnLoc, map );

							Timer.DelayCall( TimeSpan.FromSeconds( 1 ), delegate()
							{
								bc.Home = Location;
								bc.RangeHome = m_SpawnRange;
								bc.FightMode = FightMode.Closest;

								bc.MoveToWorld( spawnLoc, map );

								DoEffect( spawnLoc, map );

								bc.ForceReacquire();
							} );

							m_NextSpawn = DateTime.Now + m_NextSpawnDelay;
						}
					}
					else
					{
						PublicOverheadMessage( MessageType.Regular, 0x3B2, 500760 ); // The brazier fizzes and pops, but nothing seems to happen.
					}
				}
				catch
				{
				}
			}
			else
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
			}
		}
	}
}
