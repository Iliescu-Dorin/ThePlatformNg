namespace Core.SharedKernel.GlobalVar;
/// <summary>
/// Permission variable configuration
/// </summary>
public static class Permissions
{
    public const string Name = "Permission";

    /// <summary>
    /// Test gateway authorization
    /// You can use the 'test' user in the Blog.Core project
    /// Account: test
    /// Password: test
    /// </summary>
    public const string GWName = "GW";

    /// <summary>
    /// Whether the current project uses IDS4 permission scheme
    /// true: IDS4 is enabled
    /// false: JWT is used
    /// </summary>
    public static bool IsUseIds4 = false;

    /// <summary>
    /// Whether the current project uses Authing permission scheme
    /// true: Enabled
    /// false: JWT is used
    /// </summary>
    public static bool IsUseAuthing = false;
}

/// <summary>
/// Route variable prefix configuration
/// </summary>
public static class RoutePrefix
{
    /// <summary>
    /// Prefix name
    /// If not needed, it's better to leave it empty and avoid modification
    /// Unless it's necessary to add a specific prefix to all APIs
    /// The prefix is configured in appsettings.json
    /// </summary>
    public static string Name = "";
}

/// <summary>
/// RedisMqKey
/// </summary>
public static class RedisMqKey
{
    public const string Loging = "Loging";
}
