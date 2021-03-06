namespace Server.Items
{
    public class ChickenLeg : Food
    {

        [Constructible]
public ChickenLeg() : this( 1 )
        {
        }


        [Constructible]
public ChickenLeg( int amount ) : base( amount, 0x1608 )
        {
            Weight = 1.0;
            FillFactor = 4;
        }

        [Constructible]
public ChickenLeg( Serial serial ) : base( serial )
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
