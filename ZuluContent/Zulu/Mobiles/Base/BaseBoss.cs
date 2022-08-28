using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class BaseBoss : BaseCreatureTemplate
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public int BossPhase { get; set; } = 1;
        
        public Mobile LastAggressor { get; set; }

        public bool CheckReset { get; set; } = false;

        public virtual Rectangle2D Bounds { get; }
        
        public override bool CanFlee => false;

        [Constructible]
        public BaseBoss(string templateName) : base(templateName)
        {
        }

        public BaseBoss(Serial serial) : base(serial)
        {
        }

        public void ResetFight()
        {
            Spawner.Respawn();
        }

        public bool ShouldReset()
        {
            return Combatant == null && Warmode == false && Utility.RandomDouble() < .10;
        }

        public virtual void OnBossPhaseThink()
        {
            if (!Bounds.Contains(Location))
            {
                ResetFight();
                return;
            }

            if (CheckReset && ShouldReset())
            {
                if (LastAggressor != null)
                {
                    RemoveAggressor(LastAggressor);
                }

                if (Aggressors.Count > 0)
                {
                    LastAggressor = Aggressors[0].Attacker;
                    AggressiveAction(LastAggressor, false);
                }
                else
                {
                    ResetFight();
                }
            }
        }

        public override void OnThink()
        {
            OnBossPhaseThink();
        }
        
        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();

            Timer.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                Spawner?.Respawn();
            });
        }
    }
}