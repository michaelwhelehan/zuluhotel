using Server.Guilds;

namespace Server.Items
{
    public class OrderShield : BaseShield
	{
		public override int InitMinHits{ get{ return 100; } }
		public override int InitMaxHits{ get{ return 125; } }

		public override int ArmorBase{ get{ return 30; } }


		[Constructible]
public OrderShield() : base( 0x1BC4 )
		{
			LootType = LootType.Newbied;

			Weight = 7.0;
		}

		[Constructible]
public OrderShield( Serial serial ) : base(serial)
		{
		}

		public override void Deserialize( IGenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Weight == 6.0 )
				Weight = 7.0;
		}

		public override void Serialize( IGenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );//version
		}

		public override bool OnEquip( Mobile from )
		{
			return Validate( from ) && base.OnEquip( from );
		}

		public override void OnSingleClick( Mobile from )
		{
			if ( Validate( Parent as Mobile ) )
				base.OnSingleClick( from );
		}

		public virtual bool Validate( Mobile m )
		{
			if ( m == null || !m.Player || m.AccessLevel != AccessLevel.Player )
				return true;

			Guild g = m.Guild as Guild;

			if ( g == null || g.Type != GuildType.Order )
			{
				m.FixedEffect( 0x3728, 10, 13 );
				Delete();

				return false;
			}

			return true;
		}
	}
}
