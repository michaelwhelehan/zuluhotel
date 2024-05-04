using Server;
using Server.Engines.Craft;
using Server.Items;

namespace ZuluContent.Zulu.Items.SingleClick
{
    public static partial class SingleClickHandler
    {
        public static void HandleSingleClick(PowerCombineBeads item, Mobile m)
        {
            DefaultHandleSingleClick(item, m);
        }
    }
}