using Server.Items;
using Server.Mobiles;

namespace Server.Multis
{
    public class MageCamp : BaseCamp
	{

		[Constructible]
public MageCamp() : base( 0x1F5 )
		{
		}

		public override void AddComponents()
		{
			BaseDoor west, east;

			AddItem( west = new LightWoodGate( DoorFacing.WestCW ), -4, 4, 7 );
			AddItem( east = new LightWoodGate( DoorFacing.EastCCW ), -3, 4, 7 );

			west.Link = east;
			east.Link = west;

			AddItem( new Sign( SignType.Mage, SignFacing.West ), -5, 5, -4 );

			AddMobile( new Mage(), 4, -4,  3, 7 );
			AddMobile( new Mage(), 5,  4, -2, 0 );
		}

		[Constructible]
public MageCamp( Serial serial ) : base( serial )
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
