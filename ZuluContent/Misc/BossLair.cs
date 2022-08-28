using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Scripts.Zulu.Utilities;
using Server.Json;
using Server.Logging;
using Server.Mobiles;
using Server.Regions;
using Server.Utilities;

namespace Server;

public class BossLair : BaseRegion
{
    private static readonly ILogger logger = LogFactory.GetLogger(typeof(BossLair));

    private string LairName { get; }
    
    public BossLair(int x, int y, int width, int height, Map map, bool allowMounts, string lairName) : base(null, map,
        DefaultPriority,
        new Rectangle2D(x, y, width, height))
    {
        AllowMounts = allowMounts;
        LairName = lairName;

        GoLocation = new Point3D(x, y, map.GetAverageZ(x, y));

        Register();
    }
    
    public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
    {
        global = LightCycle.DungeonLevel;
    }

    public static void Initialize()
    {
        var path = Path.Join(Core.BaseDirectory, "Data/bosslairs.json");

        var failures = new List<string>();
        var count = 0;

        logger.Information("Loading boss lairs");

        var stopwatch = Stopwatch.StartNew();
        var regions = JsonConfig.Deserialize<List<DynamicJson>>(path);
        if (regions == null) throw new JsonException($"Failed to deserialize {path}.");

        foreach (var json in regions)
        {
            var type = AssemblyHandler.FindTypeByName(json.Type);

            if (type == null)
            {
                failures.Add($"\tInvalid boss lair type {json.Type}");
                continue;
            }

            var options = JsonConfig.DefaultOptions;
            var lairMap = json.GetProperty("map", options, out Map map) ? map : null;
            var lairBounds = json.GetProperty("bounds", options, out List<int> bounds)
                ? bounds.ToArray()
                : Array.Empty<int>();
            var allowMounts = json.GetProperty("mounts", options, out bool mounts) && mounts;
            var lairName = json.GetProperty("name", options, out string name) ? name : null;

            type.CreateInstance<BossLair>(lairBounds[0], lairBounds[1], lairBounds[2], lairBounds[3], lairMap,
                allowMounts, lairName);
            count++;
        }

        stopwatch.Stop();

        if (failures.Count == 0)
        {
            logger.Information(
                "Boss lairs loaded ({0} lairs, {1} failures) ({2:F2} seconds)",
                count,
                failures.Count,
                stopwatch.Elapsed.TotalSeconds
            );
        }
        else
        {
            logger.Warning(
                "Failed loading lairs ({0} lairs, {1} failures) ({2:F2} seconds)",
                count,
                failures.Count,
                stopwatch.Elapsed.TotalSeconds
            );

            logger.Warning(string.Join(Environment.NewLine, failures));
        }
    }

    public override void OnEnter(Mobile m)
    {
        base.OnEnter(m);
        
        m.SendSuccessMessage($"You have entered {LairName}");
    }

    public override void OnExit(Mobile m)
    {
        base.OnExit(m);
        
        m.SendSuccessMessage($"You have exited {LairName}");
    }
}