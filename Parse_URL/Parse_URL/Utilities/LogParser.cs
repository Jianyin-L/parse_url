using Parse_URL.Model;
using System.Text.RegularExpressions;

namespace Parse_URL.Utilities;

public class LogParser
{
    private static readonly Regex LogPattern = new Regex(
        @"(?<ip>\d+\.\d+\.\d+\.\d+) - - \[(?<timestamp>[^\]]+)] ""(?<method>GET|POST|PUT|DELETE) (?<url>[^\s]+) .*"" (?<status>\d+) (?<size>\d+|-) "".*"" ""(?<useragent>.*)""",
        RegexOptions.Compiled);

    public List<LogEntry> ParseLogFile(string filePath)
    {
        var logEntries = new List<LogEntry>();

        foreach (var line in File.ReadLines(filePath))
        {
            var match = LogPattern.Match(line);
            if (!match.Success) continue;

            logEntries.Add(new LogEntry
            {
                IPAddress = match.Groups["ip"].Value,
                Timestamp = DateTime.TryParse(match.Groups["timestamp"].Value, out var dt) ? dt : DateTime.MinValue,
                HttpMethod = match.Groups["method"].Value,
                Url = match.Groups["url"].Value,
                StatusCode = int.TryParse(match.Groups["status"].Value, out var status) ? status : 0,
                ResponseSize = int.TryParse(match.Groups["size"].Value, out var size) ? size : 0,
                UserAgent = match.Groups["useragent"].Value
            });
        }

        return logEntries;
    }
}