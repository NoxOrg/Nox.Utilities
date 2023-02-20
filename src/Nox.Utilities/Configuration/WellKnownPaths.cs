using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nox.Utilities.Configuration;

public static class WellKnownPaths
{
    public static readonly string CliCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "nox");
    public static readonly string CachePath = Utilities.Configuration.WellKnownPaths.CliCachePath;
    public static readonly string CacheFile = Path.Combine(CachePath, "NoxCliCache.json");
    public static readonly string CacheTokenFile = Path.Combine(CachePath, "NoxCliCache-token.json");
    public static readonly string WorkflowsCachePath = Path.Combine(CachePath, "workflows");
}
