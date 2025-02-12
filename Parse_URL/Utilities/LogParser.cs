using Parse_URL.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Parse_URL.Utilities;

public class LogParser
{
    private static readonly Regex LogPattern = new Regex(
        @"(?<ip>\d+\.\d+\.\d+\.\d+|-)\s+-\s+(?<user>\S+)\s+\[(?<timestamp>[^\]]+)]\s+""(?<method>[^\s]+)\s+(?<url>[^\s]+).*""\s+(?<status>\d{3})\s+(?<size>\d+|-)\s+"".*""\s+""(?<useragent>.*)""",   // TODO: missing value in timestamp, status code etc..
        RegexOptions.Compiled);

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
        var isHttpMethod = Enum.TryParse<Models.HttpMethod>(match.Groups["method"].Value.ToUpper(), out var method);

        return !match.Success
            || !isHttpMethod
            ? null
            : new LogEntry
            {
                IPAddress = match.Groups["ip"].Value,
                User = match.Groups["user"].Value,
                Timestamp = DateTimeOffset.TryParseExact(match.Groups["timestamp"].Value, "dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : DateTimeOffset.MinValue,
                Method = method,
                Url = match.Groups["url"].Value,
                StatusCode = int.TryParse(match.Groups["status"].Value, out var status) ? status : 0,
                ResponseSize = int.TryParse(match.Groups["size"].Value, out var size) ? size : 0,
                UserAgent = match.Groups["useragent"].Value
            };
    }
}