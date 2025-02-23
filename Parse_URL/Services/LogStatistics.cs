using Parse_URL.Models;

namespace Parse_URL.Services;

/// <summary>
/// Provides methods for calculating statistics on log entries.
/// </summary>
public static class LogStatistics
{
    /// <summary>
    /// Counts the number of unique items in the log entries based on the specified selector.
    /// </summary>
    /// <typeparam name="T">The type of the selected item.</typeparam>
    /// <param name="logEntries">The list of log entries.</param>
    /// <param name="selector">The selector function to extract the item from each log entry.</param>
    /// <param name="filterMissing">A flag indicating whether to filter out missing values.</param>
    /// <returns>The count of unique items.</returns>
    public static int CountUniqueItems<T>(List<LogEntry> logEntries, Func<LogEntry, T> selector, bool filterMissing = false)
    {
        return logEntries
            .Select(selector)
            .Where(value => !filterMissing || !IsMissing(value))
            .Distinct()
            .Count();
    }

    /// <summary>
    /// Gets the top items in the log entries based on the specified selector and count.
    /// </summary>
    /// <typeparam name="T">The type of the selected item.</typeparam>
    /// <param name="logEntries">The list of log entries.</param>
    /// <param name="selector">The selector function to extract the item from each log entry.</param>
    /// <param name="n">The number of top items to retrieve.</param>
    /// <param name="filterMissing">A flag indicating whether to filter out missing values.</param>
    /// <param name="includeTies">A flag indicating whether to include ties in the result.</param>
    /// <returns>A dictionary containing the top items and their counts.</returns>
    public static Dictionary<string, int> GetTopItems<T>(List<LogEntry> logEntries, Func<LogEntry, T> selector, int n, bool filterMissing = false, bool includeTies = false)
    {
        var grouped = logEntries
            .Select(entry => selector(entry))
            .Where(value => !filterMissing || !IsMissing(value))
            .GroupBy(value => value)
            .OrderByDescending(g => g.Count());

        IEnumerable<IGrouping<T, T>>? result;

        if (!includeTies)
        {
            result = grouped.Take(n);
        }
        else
        {
            var minCountToInclude = grouped.Select(g => g.Count())
                .Distinct()
                .Take(n)
                .LastOrDefault();

            result = grouped
                .Where(g => g.Count() >= minCountToInclude);
        }

        return result.ToDictionary(g => g.Key?.ToString() ?? "Unknown", g => g.Count() );
    }

    /// <summary>
    /// Checks if a value is considered missing based on its type.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is considered missing, otherwise false.</returns>
    private static bool IsMissing<T>(T value)
    {
        return value switch
        {
            null => true,
            string str => string.IsNullOrWhiteSpace(str),
            DateTimeOffset dto => dto == DateTimeOffset.MinValue,
            int num => num == 0,
            _ => false
        };
    }
}