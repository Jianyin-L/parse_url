using Parse_URL.Model;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Parse_URL.Utilities;

public class LogParser
{
    private static readonly Regex LogPattern = new Regex(
        @"(?<ip>\d+\.\d+\.\d+\.\d+|-) - (?<user>.+) \[(?<timestamp>[^\]]+)] ""(?<method>[^\s]+) (?<url>[^\s]+) .*"" (?<status>\d{3}) (?<size>\d+|-) "".*"" ""(?<useragent>.*)""",
        RegexOptions.Compiled);

    private static readonly string[] HttpMethods = ["GET", "POST", "PUT", "DELETE", "PATCH", "HEAD", "OPTIONS", "TRACE"];  // TODO: Turn this to an enum??     // Assuming these are the only valid HTTP methods

    public static List<LogEntry> ParseLogFile(string filePath)
    {
        var logEntries = new List<LogEntry>();

        foreach (var line in File.ReadLines(filePath))
        {
            var entry = ParseLogLine(line);
            if (entry != null)
            {
                logEntries.Add(entry);
            }
        }

        return logEntries;
    }

    private static LogEntry? ParseLogLine(string entry)
    {
        var match = LogPattern.Match(entry);

        return !match.Success
            || !HttpMethods.Contains(match.Groups["method"].Value, StringComparer.OrdinalIgnoreCase)
            ? null
            : new LogEntry
            {
                IPAddress = match.Groups["ip"].Value,
                User = match.Groups["user"].Value,
                Timestamp = DateTimeOffset.TryParseExact(match.Groups["timestamp"].Value, "dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : DateTimeOffset.MinValue,
                HttpMethod = match.Groups["method"].Value,
                Url = match.Groups["url"].Value,
                StatusCode = int.TryParse(match.Groups["status"].Value, out var status) ? status : 0,
                ResponseSize = int.TryParse(match.Groups["size"].Value, out var size) ? size : 0,
                UserAgent = match.Groups["useragent"].Value
            };
    }
}