namespace Server.Items
{
    public class Bandana : BaseHat
    {
        public override int InitMinHits{ get{ return 20; } }
        public override int InitMaxHits{ get{ return 30; } }


        [Constructible]
public Bandana() : this( 0 )
        {
        }


        [Constructible]
public Bandana( int hue ) : base( 0x1540, hue )
        {
            Weight = 1.0;
        }

        [Constructible]
public Bandana( Serial serial ) : base( serial )
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
