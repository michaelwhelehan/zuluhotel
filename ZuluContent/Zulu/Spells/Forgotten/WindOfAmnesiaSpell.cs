using System.Threading.Tasks;
using Server;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class WindOfAmnesiaSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        public WindOfAmnesiaSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            var loc = SpellHelper.GetSurfaceTop(target.Location);
            
            SpellHelper.Turn(Caster, target);
            
            Effects.SendLocationParticles(
                EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration),
                0x3728, 8, 20, 5042
            );
            Caster.PlaySound(0x0FE);
            
            var magery = Caster.Skills[SkillName.Magery].Value / 10;

            var kickbackX = magery > 0 ? Utility.Random((int) magery) - (int) (magery / 2) : 0;
            var kickbackY = magery > 0 ? Utility.Random((int) magery) - (int) (magery / 2) : 0;
            
            var newTargetLocation = new Point3D(target.Location.X + kickbackX, target.Location.Y + kickbackY,
                target.Location.Z);
            var map = target.Map;

            if (map.LineOfSight(Caster, newTargetLocation) && map.CanSpawnMobile(newTargetLocation))
                target.MoveToWorld(newTargetLocation, map);
            
            var damage = SpellHelper.CalcSpellDamage(Caster, target, this);
            SpellHelper.Damage(damage, target, Caster, this);
        }
    }
}