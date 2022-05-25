using System;
using Scripts.Zulu.Utilities;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Commands
{
  public class PowerHour
  {
    public static void Initialize()
    {
      CommandSystem.Register("PowerHour", AccessLevel.Player, PowerHour_OnCommand);
    }

    [Usage("PowerHour")]
    [Description("Enables power hour if it is ready")]
    private static void PowerHour_OnCommand(CommandEventArgs e)
    {
        if (e.Mobile is PlayerMobile player)
        {
            var nextPowerHour = player.LastPowerHourUsed + TimeSpan.FromDays(1);
            var timeUntilNextPowerHour = nextPowerHour - Core.Now;

            if (timeUntilNextPowerHour.TotalHours >= 23.0)
            {
                player.SendSuccessMessage($"Your power hour has begun. {timeUntilNextPowerHour.Minutes} minutes remaining.");
            }
            else if (timeUntilNextPowerHour.TotalDays <= 0.0)
            {
                player.CloseGump<WarningGump>();
                player.SendGump(
                    new WarningGump(1060637, 30720,
                        "Would you like to start your power hour now?",
                        0xFFC000, 360, 260, okay => StartPowerHour_Callback(player, okay)));
            }
            else
            {
                player.SendSuccessMessage($"Your next power hour will be available in {timeUntilNextPowerHour.Hours + 1} hours {timeUntilNextPowerHour.Minutes} minutes.");
            }
        }
    }
    
    public static void StartPowerHour_Callback(PlayerMobile player, bool okay)
    {
        if (okay)
        {
            player.LastPowerHourUsed = Core.Now;
            player.SendSuccessMessage("Your power hour has begun.");
        }
    }
  }
}