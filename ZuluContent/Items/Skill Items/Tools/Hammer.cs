using Server.Engines.Craft;

namespace Server.Items
{
    public class Hammer : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefCarpentry.CraftSystem; } }


		[Constructible]
public Hammer() : base( 0x102A )
		{
			Weight = 2.0;
		}


		[Constructible]
public Hammer( int uses ) : base( uses, 0x102A )
		{
			Weight = 2.0;
		}

		[Constructible]
public Hammer( Serial serial ) : base( serial )
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
