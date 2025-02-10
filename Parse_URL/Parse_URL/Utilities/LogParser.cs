using Parse_URL.Model;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Parse_URL.Utilities;

public class LogParser
{
    private static readonly Regex LogPattern = new Regex(
        @"(?<ip>\d+\.\d+\.\d+\.\d+|-) - (?<user>.+) \[(?<timestamp>[^\]]+)] ""(?<method>[^\s]+) (?<url>[^\s]+) .*"" (?<status>\d+) (?<size>\d+|-) "".*"" ""(?<useragent>.*)""",
        RegexOptions.Compiled);

    private static readonly string[] HttpMethods = ["GET", "POST", "PUT", "DELETE", "PATCH", "HEAD", "OPTIONS", "TRACE", "PATCH"];  // TODO: Turn this to an enum??     // Assuming these are the only valid HTTP methods

    public static List<LogEntry> ParseLogFile(string filePath)
    {
        var logEntries = new List<LogEntry>();

        foreach (var line in File.ReadLines(filePath))
        {
            var match = LogPattern.Match(line);

            if (!match.Success
                || !HttpMethods.Contains(match.Groups["method"].Value, StringComparer.OrdinalIgnoreCase))
                continue;

            logEntries.Add(new LogEntry
            {
                IPAddress = match.Groups["ip"].Value,
                User = match.Groups["user"].Value,
                Timestamp = DateTimeOffset.TryParseExact(match.Groups["timestamp"].Value, "dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : DateTimeOffset.MinValue,
                HttpMethod = match.Groups["method"].Value,
                Url = match.Groups["url"].Value,
                StatusCode = int.TryParse(match.Groups["status"].Value, out var status) ? status : 0,
                ResponseSize = int.TryParse(match.Groups["size"].Value, out var size) ? size : 0,
                UserAgent = match.Groups["useragent"].Value
            });
        }

        return logEntries;
    }

    public static int CountUniqueItems<T>(List<LogEntry> logEntries, Func<LogEntry, T> selector)
    {
        return logEntries.Select(selector).Distinct().Count();
    }

    public static Dictionary<string, int> GetTopItems<T>(List<LogEntry> logEntries, Func<LogEntry, T> selector, int n)
    {
        return logEntries
            .GroupBy(selector)
            .OrderByDescending(g => g.Count())
            .Take(n)
            .ToDictionary(g => g.Key?.ToString() ?? "Unknown", g => g.Count());
    }

    public static Dictionary<string, int> GetTopItemsIncludingTies<T>(List<LogEntry> logEntries, Func<LogEntry, T> selector, int n)
    {
        var grouped = logEntries
            .GroupBy(selector)
            .Select(g => new { Key = g.Key?.ToString() ?? "Unknown", Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();

        var minCountToInclude = grouped.Select(g => g.Count)
            .Distinct()
            .Take(n)
            .LastOrDefault();

        return grouped
            .Where(g => g.Count >= minCountToInclude)
            .ToDictionary(g => g.Key!, g => g.Count);
    }
}