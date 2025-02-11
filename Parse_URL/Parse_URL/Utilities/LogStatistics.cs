using Parse_URL.Model;

namespace Parse_URL.Utilities;

public class LogStatistics
{
    public static int CountUniqueItems<T>(List<LogEntry> logEntries, Func<LogEntry, T> selector, bool filterMissing = false)
    {
        return logEntries
            .Select(selector)
            .Where(value => !filterMissing || !IsMissing(value))
            .Distinct()
            .Count();
    }

    public static Dictionary<string, int> GetTopItems<T>(List<LogEntry> logEntries, Func<LogEntry, T> selector, int n, bool filterMissing = false)
    {
        return logEntries
            .Select(entry => selector(entry))
            .Where(value => !filterMissing || !IsMissing(value))
            .GroupBy(value => value)
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

    private static bool IsMissing<T>(T value)
    {
        return value switch
        {
            null => true, // Null values are missing
            string str => string.IsNullOrWhiteSpace(str), // Empty or whitespace-only strings are missing
            DateTimeOffset dto => dto == DateTimeOffset.MinValue, // MinValue for timestamps
            int num => num == 0, // Consider 0 as missing if needed
            _ => false // Other types are assumed to be valid
        };
    }
}