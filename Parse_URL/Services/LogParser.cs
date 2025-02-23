using Parse_URL.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Parse_URL.Services;

/// <summary>
/// The LogParser class is responsible for parsing log files and extracting log entries.
/// </summary>
public class LogParser
{
    /// <summary>
    /// Regular expression pattern used to match log entries in a log file.
    /// </summary>
    private static readonly Regex LogPattern = new Regex(
        @"(?<ip>-|\d+\.\d+\.\d+\.\d+)\s+-\s+(?<user>\S+)\s+\[(?<timestamp>-|[^\]]+)]\s+""(?<method>-|[^\s]+)\s+(?<url>-|[^\s]+|).*""\s+(?<status>-|\d{3})\s+(?<size>-|\d+)\s+"".*""\s+""(?<useragent>.*)""",
        RegexOptions.Compiled);

    /// <summary>
    /// Parses a log file and returns a list of log entries.
    /// </summary>
    /// <param name="filePath">The path to the log file.</param>
    /// <returns>A list of log entries.</returns>
    public static List<LogEntry> ParseLogFile(string filePath)
    {
        var logEntries = new List<LogEntry>();
        Console.WriteLine("Start parsing log file...");

        foreach (var line in File.ReadLines(filePath))
        {
            var entry = ParseLogLine(line);
            if (entry != null)
            {
                logEntries.Add(entry);
            }
        }

        Console.WriteLine("Completed!");
        return logEntries;
    }

    /// <summary>
    /// Parses a single log line and returns a log entry object.
    /// </summary>
    /// <param name="entry">The log line to parse.</param>
    /// <returns>A log entry object if the line is successfully parsed, otherwise null.</returns>
    private static LogEntry? ParseLogLine(string entry)
    {
        var match = LogPattern.Match(entry);

        var methodString = match.Groups["method"].Value;
        var isHttpMethod = Enum.TryParse(
            methodString.Equals("-", StringComparison.CurrentCultureIgnoreCase) ? "MISSING" : methodString.ToUpper(),
            out Models.HttpMethod method);

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