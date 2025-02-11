﻿using Parse_URL.Model;

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

    public static Dictionary<string, int> GetTopItems<T>(List<LogEntry> logEntries, Func<LogEntry, T> selector, int n, bool filterMissing = false, bool includeTies = false)
    {
        var grouped = logEntries
            .Select(entry => selector(entry))
            .Where(value => !filterMissing || !IsMissing(value))
            .GroupBy(value => value)
            .Select(g => new { Key = g.Key?.ToString() ?? "Unknown", Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();

        if (!includeTies)
        {
            return grouped
                .Take(n)
                .ToDictionary(g => g.Key, g => g.Count);
        }
        else
        {
            var minCountToInclude = grouped.Select(g => g.Count)
                .Distinct()
                .Take(n)
                .LastOrDefault();

            return grouped
                .Where(g => g.Count >= minCountToInclude)
                .ToDictionary(g => g.Key!, g => g.Count);
        }

    }

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