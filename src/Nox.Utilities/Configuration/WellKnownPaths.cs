using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nox.Utilities.Configuration;

public static class WellKnownPaths
{
    private static readonly string _userApps = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public static readonly string CachePath = Path.Combine(_userApps, "nox");
    public static readonly string CacheFile = Path.Combine(CachePath, "NoxCliCache.json");
    public static readonly string WorkflowsCachePath = Path.Combine(CachePath, "workflows");
    public static readonly string TemplatesCachePath = Path.Combine(CachePath, "templates");
    public static readonly string SecretsCachePath = Path.Combine(CachePath, "store");
    public static readonly string CacheTokenFile = Path.Combine(SecretsCachePath, "NoxCliCache-token.json");
}
