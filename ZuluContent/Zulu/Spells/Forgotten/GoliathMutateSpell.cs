using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.Zulu.Engines.Classes;
using Scripts.Zulu.Utilities;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using ZuluContent.Zulu.Engines.Magic.Enchantments.Buffs;

namespace Scripts.Zulu.Spells.Forgotten
{
    public class GoliathMutateSpell : ForgottenSpell, ITargetableAsyncSpell<Mobile>
    {
        private static readonly PolymorphEntry[] Entries =
        {
            PolymorphEntry.Titan,
            PolymorphEntry.Troll,
            PolymorphEntry.Ostard,
        };
        
        private static readonly Dictionary<int, (int Difficulty, int Points)> BodyData =
            new()
            {
                {PolymorphEntry.Titan.BodyId, (10, 100)},
                {PolymorphEntry.Troll.BodyId, (15, 150)},
                {PolymorphEntry.Ostard.BodyId, (20, 200)}
            };
        
        public GoliathMutateSpell(Mobile caster, Item spellItem = null) : base(caster, spellItem) { }

        public async Task OnTargetAsync(ITargetResponse<Mobile> response)
        {
            if (!response.HasValue)
                return;
            
            var target = response.Target;
            SpellHelper.Turn(Caster, target);

            if (target is not PlayerMobile)
                return;
            
            if (!Caster.CanBuff(target, true, BuffIcon.Polymorph, BuffIcon.LichForm, BuffIcon.WraithForm, BuffIcon.StoneForm))
                return;
            
            var bodyId = await new NewPolymorphGump(Caster, Entries);

            if (bodyId <= 0)
                return;

            if (!Caster.ShilCheckSkill(SkillName.Meditation, BodyData[bodyId].Difficulty, BodyData[bodyId].Points))
            {
                Caster.SendFailureMessage("You fail to transform your target.");
                DoFizzle();
                return;
            }

            var hueMod = 0;
            
            target.FixedParticles(0x373A, 10, 10, 5007, EffectLayer.Waist);
            target.PlaySound(0x209);

            var statMod = (int) (SpellHelper.GetModAmount(Caster, target, StatType.All) * 0.8);
            
            target.TryAddBuff(new Polymorph
            {
                Title = "Goliath Mutate",
                Icon = BuffIcon.StoneForm,
                Duration = SpellHelper.GetDuration(Caster, Caster),
                BodyMods = (body: bodyId, bodyHue: hueMod),
                StatMods = (StrMod: statMod, DexMod: statMod, IntMod: statMod)
            });
        }
    }
}