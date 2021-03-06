using System;

namespace Server.Items
{
    public class MessageInABottle : Item
	{
		public static int GetRandomLevel()
		{
			return Utility.RandomMinMax( 1, 3 );
		}

		public override int LabelNumber{ get{ return 1041080; } } // a message in a bottle

		private Map m_TargetMap;
		private int m_Level;

		[CommandProperty( AccessLevel.GameMaster )]
		public Map TargetMap
		{
			get{ return m_TargetMap; }
			set{ m_TargetMap = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Level
		{
			get{ return m_Level; }
			set{ m_Level = Math.Max( 1, Math.Min( value, 4 ) ); }
		}


		[Constructible]
public MessageInABottle() : this( Map.Felucca )
		{
		}

		[Constructible]
public MessageInABottle( Map map ) : this( map, GetRandomLevel() )
		{
		}


		[Constructible]
public MessageInABottle( Map map, int level ) : base( 0x099F )
		{
			Weight = 1.0;
			m_TargetMap = map;
			m_Level = level;
		}

		[Constructible]
public MessageInABottle( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( IGenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version

			writer.Write( (int) m_Level );

			writer.Write( m_TargetMap );
		}

		public override void Deserialize( IGenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
				case 2:
				{
					m_Level = reader.ReadInt();
					goto case 1;
				}
				case 1:
				{
					m_TargetMap = reader.ReadMap();
					break;
				}
				case 0:
				{
					m_TargetMap = Map.Felucca;
					break;
				}
			}

			if ( version < 2 )
				m_Level = GetRandomLevel();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				ReplaceWith( new SOS( m_TargetMap, m_Level ) );
				from.LocalOverheadMessage( Network.MessageType.Regular, 0x3B2, 501891 ); // You extract the message from the bottle.
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}
	}
}
