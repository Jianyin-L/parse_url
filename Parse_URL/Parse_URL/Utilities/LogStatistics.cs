using Parse_URL.Model;

namespace Parse_URL.Utilities;

public class LogStatistics
{
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

    public static Dictionary<string, int> GetTopItemsFilteringMissing<T>(List<LogEntry> logEntries, Func<LogEntry, T> selector, int n)
    {
        return logEntries
            .GroupBy(entry =>
                {
                    var key = selector(entry);
                    return key switch
                    { 
                        null => "", // Handle null values
                        string str when string.IsNullOrWhiteSpace(str) => "", // Handle empty strings
                        DateTimeOffset dto => dto.ToString("yyyy-MM-dd"), // Normalise dates for readability
                        _ => key.ToString() ?? "" // Convert other types safely
                    };
                })
            .Where(g => g.Key != "")
            .OrderByDescending(g => g.Count())
            .Take(n)
            .ToDictionary(g => g.Key, g => g.Count());
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