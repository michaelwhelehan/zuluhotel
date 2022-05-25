using System.Threading.Tasks;
using Server;
using Server.Spells;
using Server.Targeting;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class LevitateSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        public LevitateSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            SpellHelper.Turn(Caster, target);
            
            Caster.PlaySound(0x103);

            var location = target.Location;
            target.MoveToWorld(new Point3D(location.X, location.Y, location.Z + 20), target.Map);
        }
    }
}