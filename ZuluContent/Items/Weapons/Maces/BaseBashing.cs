namespace Server.Items
{
    public abstract class BaseBashing : BaseMeleeWeapon
	{
		public override int DefaultHitSound{ get{ return 0x233; } }
		public override int DefaultMissSound{ get{ return 0x239; } }

		public override SkillName DefaultSkill{ get{ return SkillName.Macing; } }
		public override WeaponType DefaultWeaponType{ get{ return WeaponType.Bashing; } }
		public override WeaponAnimation DefaultAnimation{ get{ return WeaponAnimation.Bash1H; } }

		public BaseBashing( int itemID ) : base( itemID )
		{
		}

		public BaseBashing( Serial serial ) : base( serial )
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

		public override double GetBaseDamage( Mobile attacker )
		{
			double damage = base.GetBaseDamage( attacker );

			if (  (attacker.Player || attacker.Body.IsHuman) && Layer == Layer.TwoHanded && attacker.Skills[SkillName.Anatomy].Value / 400.0 >= Utility.RandomDouble() )
			{
				damage *= 1.5;

				attacker.SendMessage( "You deliver a crushing blow!" ); // Is this not localized?
				attacker.PlaySound( 0x11C );
			}

			return damage;
		}
	}
}
