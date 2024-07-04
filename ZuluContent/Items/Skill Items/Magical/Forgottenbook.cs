using System;
using System.Collections.Generic;
using Scripts.Zulu.Spells.Forgotten;
using Server.Commands;
using Server.Gumps;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
    public class Forgottenbook : CustomSpellbook
    {
        public override int Hue { get; set; } = 0x46F;
        public override string DefaultName { get; } = "Book of the Forgotten";
        public override int BookOffset { get; } = 700;

        public override Dictionary<SpellEntry, int> SpellIcons { get; } = new()
        {
            { SpellEntry.FireSerpent, 23016 }, { SpellEntry.ThorsHammer, 23017 }, { SpellEntry.EnergySphere, 23018 },
            { SpellEntry.ChaoticFlux, 23019 }, { SpellEntry.WindOfAmnesia, 23020 }, { SpellEntry.MassAttack, 23021 },
            { SpellEntry.ChantOfLife, 23022 }, { SpellEntry.ChantOfEnhance, 23023 }, { SpellEntry.PoisonCloud, 23024 },
            { SpellEntry.Levitate, 23025 }, { SpellEntry.SpectralAura, 23026 }, { SpellEntry.UndeadBane, 23027 },
            { SpellEntry.GoliathMutate, 23028 }, { SpellEntry.Invocation, 23029 }, { SpellEntry.Possession, 23030 },
            { SpellEntry.MagicRune, 23031 }
        };
        
        public override Type SpellType { get; } = typeof(ForgottenSpell);

        [Constructible]
        public Forgottenbook() : base(0x0FBD) { }

        [Constructible]
        public Forgottenbook(Serial serial) : base(serial) { }
        
        public override void OnOpenSpellbook(Mobile from)
        {
            from.SendGump(new CustomSpellbookGump(from, this, 0x2B29));
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
        }
        
        #region Commands
        public static void Initialize()
        {
            CommandSystem.Register("AllForgottenSpells", AccessLevel.GameMaster, AllSpells_OnCommand);
        }

        [Usage("AllForgottenSpells")]
        [Description("Completely fills a targeted Forgotten Book with scrolls.")]
        private static void AllSpells_OnCommand(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, AllSpells_OnTarget);
            e.Mobile.SendMessage("Target the Forgotten Book to fill.");
        }

        private static void AllSpells_OnTarget(Mobile from, object obj)
        {
            if (obj is Forgottenbook book)
            {
                if (book.BookCount == 16)
                    book.Content = ulong.MaxValue;
                else
                    book.Content = (1ul << book.BookCount) - 1;

                from.SendMessage("The Forgotten Book has been filled.");

                CommandLogging.WriteLine(from, "{0} {1} filling Forgotten Book {2}", from.AccessLevel,
                    CommandLogging.Format(from), CommandLogging.Format(book));
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, AllSpells_OnTarget);
                from.SendMessage("That is not a Forgotten Book. Try again.");
            }
        }
        #endregion
    }
}