using Server.Engines.Craft;

namespace Server.Items
{
    [Flipable( 0x1EB8, 0x1EB9 )]
	public class TinkerTools : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefTinkering.CraftSystem; } }


		[Constructible]
public TinkerTools() : base( 0x1EB8 )
		{
			Weight = 1.0;
		}


		[Constructible]
public TinkerTools( int uses ) : base( uses, 0x1EB8 )
		{
			Weight = 1.0;
		}

		[Constructible]
public TinkerTools( Serial serial ) : base( serial )
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

	public class TinkersTools : BaseTool
	{
		public override CraftSystem CraftSystem { get { return DefTinkering.CraftSystem; } }


		public TinkersTools()
			: base(0x1EBC)
		{
			Weight = 1.0;
		}


		public TinkersTools(int uses)
			: base(uses, 0x1EBC)
		{
			Weight = 1.0;
		}

		public TinkersTools(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(IGenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(IGenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
