using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace JwLogging
{
    public enum LogLevelKind { Debug, Info, Warning, Error, Fatal }

    public sealed class LogEntry
    {
        public DateTime Time { get; init; }
        public LogLevelKind Level { get; init; }
        /// <summary>首行消息</summary>
        public string Message { get; init; } = "";
        /// <summary>完整内容（含异常堆栈等续行）</summary>
        public string FullText { get; set; } = "";
    }

    /// <summary>
    /// 读取 AppLog 写出的日志文件。格式：
    /// "yyyy-MM-dd HH:mm:ss.fff [LVL] 消息"，之后不以时间戳开头的行属于上一条（异常堆栈、多行消息）。
    /// </summary>
    public static class LogReader
    {
        private static readonly Regex Head = new(
            @"^(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}) \[([A-Z]{3})\] ?(.*)$", RegexOptions.Compiled);

        public static List<LogEntry> Read(string path)
        {
            // 文件正被 Serilog 写入，需共享读取
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var sr = new StreamReader(fs, Encoding.UTF8);
            return Parse(sr);
        }

        public static List<LogEntry> Parse(TextReader reader)
        {
            var result = new List<LogEntry>();
            StringBuilder? body = null;
            LogEntry? current = null;

            void Flush()
            {
                if (current != null && body != null) current.FullText = body.ToString().TrimEnd();
            }

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var m = Head.Match(line);
                if (m.Success)
                {
                    Flush();
                    current = new LogEntry
                    {
                        Time = DateTime.ParseExact(m.Groups[1].Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                        Level = ToLevel(m.Groups[2].Value),
                        Message = m.Groups[3].Value,
                    };
                    body = new StringBuilder(line).AppendLine();
                    result.Add(current);
                }
                else if (body != null)
                {
                    body.AppendLine(line);
                }
            }
            Flush();
            return result;
        }

        private static LogLevelKind ToLevel(string s) => s switch
        {
            "DBG" or "VRB" => LogLevelKind.Debug,
            "INF" => LogLevelKind.Info,
            "WRN" => LogLevelKind.Warning,
            "ERR" => LogLevelKind.Error,
            "FTL" => LogLevelKind.Fatal,
            _ => LogLevelKind.Info,
        };
    }
}
