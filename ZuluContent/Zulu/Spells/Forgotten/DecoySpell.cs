using System;
using System.Threading.Tasks;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class DecoySpell : ForgottenSpell, ITargetableAsyncSpell<IPoint3D>
    {
        public DecoySpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<IPoint3D> response)
        {
            if (!response.HasValue)
                return;

            var point = SpellHelper.GetSurfaceTop(response.Target);
            
            var map = Caster.Map;
            
            if (map == null || !map.CanSpawnMobile(point.X, point.Y, point.Z))
            {
                Caster.SendLocalizedMessage(501942); // That location is blocked.
                return;
            }

            var duration = TimeSpan.FromSeconds(Utility.Random(80, 40));

            BaseCreature.Summon("Decoy", false, Caster, new Point3D(point), 0x212, duration);
        }
    }
}