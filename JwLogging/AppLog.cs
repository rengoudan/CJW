using System.Reflection;
using System.Runtime.InteropServices;
using Serilog;
using Serilog.Events;

namespace JwLogging
{
    /// <summary>
    /// 全局日志入口。日志文件位于 %LocalAppData%\RGBJW\logs\rgbjw-yyyyMMdd.log，按天滚动，保留 30 天。
    /// 其他项目只需引用 JwLogging 并调用 AppLog.Xxx，无需直接依赖 Serilog。
    /// </summary>
    public static class AppLog
    {
        public static string LogDir { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RGBJW", "logs");

        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// 程序启动时调用一次。appVersion 一般传 RGBControls.dll 的版本。
        /// </summary>
        public static void Init(string? appVersion = null)
        {
            if (IsInitialized) return;

            Directory.CreateDirectory(LogDir);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    Path.Combine(LogDir, "rgbjw-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    fileSizeLimitBytes: 50 * 1024 * 1024,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            IsInitialized = true;

            Info("===== アプリ起動 ver={Version} os={OS} runtime={Runtime} =====",
                appVersion ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "?",
                RuntimeInformation.OSDescription,
                RuntimeInformation.FrameworkDescription);
        }

        public static void Debug(string template, params object?[] args) => Log.Debug(template, args);
        public static void Info(string template, params object?[] args) => Log.Information(template, args);
        public static void Warn(string template, params object?[] args) => Log.Warning(template, args);
        public static void Warn(Exception ex, string template, params object?[] args) => Log.Warning(ex, template, args);
        public static void Error(Exception ex, string template, params object?[] args) => Log.Error(ex, template, args);
        public static void Fatal(Exception ex, string template, params object?[] args) => Log.Fatal(ex, template, args);

        public static void CloseAndFlush()
        {
            if (!IsInitialized) return;
            Info("===== アプリ終了 =====");
            Log.CloseAndFlush();
            IsInitialized = false;
        }

        /// <summary>
        /// 返回最近 days 天的日志文件（新的在前）。
        /// </summary>
        public static IEnumerable<FileInfo> GetRecentLogFiles(int days = 7)
        {
            var dir = new DirectoryInfo(LogDir);
            if (!dir.Exists) return Enumerable.Empty<FileInfo>();
            var since = DateTime.Now.Date.AddDays(-days);
            return dir.GetFiles("rgbjw-*.log")
                      .Where(f => f.LastWriteTime >= since)
                      .OrderByDescending(f => f.LastWriteTime);
        }
    }
}
