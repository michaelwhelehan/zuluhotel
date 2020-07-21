namespace Server.Items
{
    [FlipableAttribute( 0x1B09, 0x1B10 )]
	public class BonePile : Item, IScissorable
	{

		[Constructible]
public BonePile( ) : base( 0x1B09 + Utility.Random( 8 ) )
		{
			Stackable = false;
			Weight = 10.0;
		}

		[Constructible]
public BonePile( Serial serial ) : base( serial )
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

		public bool Scissor( Mobile from, Scissors scissors )
		{
			if ( Deleted || !from.CanSee( this ) )
				return false;

			base.ScissorHelper( from, new Bone(), Utility.RandomMinMax( 10, 15 ) );

			return true;
		}
	}
}