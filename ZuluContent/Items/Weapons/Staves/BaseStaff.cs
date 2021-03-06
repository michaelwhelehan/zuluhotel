namespace Server.Items
{
    public abstract class BaseStaff : BaseMeleeWeapon
	{
		public override int DefaultHitSound{ get{ return 0x233; } }
		public override int DefaultMissSound{ get{ return 0x239; } }

		public override SkillName DefaultSkill{ get{ return SkillName.Macing; } }
		public override WeaponType DefaultWeaponType{ get{ return WeaponType.Staff; } }
		public override WeaponAnimation DefaultAnimation{ get{ return WeaponAnimation.Bash2H; } }

		public BaseStaff( int itemID ) : base( itemID )
		{
		}

		public BaseStaff( Serial serial ) : base( serial )
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

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
			base.OnHit( attacker, defender, damageBonus );

			defender.Stam -= Utility.Random( 3, 3 ); // 3-5 points of stamina loss
		}
	}
}
