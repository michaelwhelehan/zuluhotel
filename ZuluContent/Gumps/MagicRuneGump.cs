using System;
using System.Runtime.CompilerServices;
using Server.Network;
namespace Server.Gumps
{
    public record MagicRuneEntry
    {
        public static readonly MagicRuneEntry HealingRune = new(1, 18518, "Magic Healing Rune", 25, 9);
        public static readonly MagicRuneEntry DamageRune = new(2, 18521, "Magic Damage Rune", 25, 11);
        
        private MagicRuneEntry(int runeId, int art, string name, int x, int y)
        {
            RuneId = runeId;
            ArtId = art;
            Name = name;
            X = x;
            Y = y;
        }

        public int RuneId { get; }
        public int ArtId { get; }
        public string Name { get; }
        public int X { get; }
        public int Y { get; }
    }

    public class MagicRuneGump : Gump, INotifyCompletion
    {
        private readonly Mobile m_Caster;
        private static readonly MagicRuneEntry[] Entries =
        {
            MagicRuneEntry.HealingRune,
            MagicRuneEntry.DamageRune
        };

        public MagicRuneGump(Mobile caster) : base(0, 0)
        {
            m_Caster = caster;

            AddPage(0);

            AddBackground(0, 0, 520, 404, 0x13BE);
            AddImageTiled(10, 10, 500, 20, 0xA40);
            AddImageTiled(10, 40, 500, 324, 0xA40);
            AddImageTiled(10, 374, 500, 20, 0xA40);
            AddAlphaRegion(10, 10, 500, 384);

            AddHtml(14, 12, 500, 20, "<BASEFONT COLOR=#FFFFFF><CENTER>Magic Rune Selection Menu<CENTER></BASEFONT>");

            AddButton(10, 374, 0xFB1, 0xFB2, 0);
            AddHtmlLocalized(45, 376, 450, 20, 1060051, 0x7FFF); // CANCEL

            for (var i = 0; i < Entries.Length; i++)
            {
                var entry = Entries[i];

                var page = i / 10 + 1;
                var pos = i % 10;

                if (pos == 0)
                {
                    if (page > 1)
                    {
                        AddButton(400, 374, 0xFA5, 0xFA7, 0, GumpButtonType.Page, page);
                        AddHtmlLocalized(440, 376, 60, 20, 1043353, 0x7FFF); // Next
                    }

                    AddPage(page);

                    if (page > 1)
                    {
                        AddButton(300, 374, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 1);
                        AddHtmlLocalized(340, 376, 60, 20, 1011393, 0x7FFF); // Back
                    }
                }

                var x = pos % 2 == 0 ? 14 : 264;
                var y = pos / 2 * 64 + 44;

                AddImageTiledButton(x, y, 0x918, 0x919, i + 1, GumpButtonType.Reply, 0, entry.ArtId, 0x0, entry.X,
                    entry.Y);
                AddHtml(x + 84, y, 250, 60, $"<BASEFONT COLOR=#FFFFFF>{entry.Name}</BASEFONT>");
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var idx = info.ButtonID - 1;

            if (idx >= 0 && idx < Entries.Length) 
                RuneId = Entries[idx].RuneId;

            IsCompleted = true;
            Continuation?.Invoke();
        }
        
        #region INotifyCompletion
        
        protected Action Continuation;
        public bool IsCompleted { get; private set; }
        public virtual MagicRuneGump GetAwaiter() => this;
        public int GetResult() => RuneId;
        public int RuneId { get; private set; } = -1;

        public void OnCompleted(Action continuation)
        {
            if (!IsCompleted && RuneId == -1)
            {
                Continuation = continuation;
                m_Caster?.SendGump(this);
            }
            else
            {
                continuation();
            }
        }
        
        #endregion
    }
}