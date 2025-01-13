using System.Reflection;

namespace WebApplication1;

public static class GlobalData
{
    static GlobalData()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var entryAssemblyName = entryAssembly?.GetName();
        ApplicationName = entryAssemblyName?.Name ?? "vote-app";
        ApplicationVersion = entryAssemblyName?.Version?.ToString() ?? "1.0.0.0";
        SourceName = $"vote-app.{ApplicationName}";
    }

    public static string ApplicationName { get; }

    public static string ApplicationVersion { get; }

    public static string SourceName { get; }
}