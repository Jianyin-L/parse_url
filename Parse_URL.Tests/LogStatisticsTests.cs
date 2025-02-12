using Parse_URL.Models;
using Parse_URL.Utilities;

namespace Parse_URL.Tests;

public class LogStatisticsTests
{
    [Fact]
    public void CountUniqueItems_ShouldReturnUniqueIPCount()
    {
        var logEntries = new List<LogEntry>
        {
            new() { IPAddress = "111.11.111.11"},
            new() { IPAddress = "111.11.111.11"},
            new() { IPAddress = "222.22.222.22"},
            new() { IPAddress = ""}
        };

        var result = LogStatistics.CountUniqueItems(logEntries, log => log.IPAddress);
        Assert.Equal(3, result);
    }

    [Fact]
    public void CountUniqueItems_ShouldReturnNumberOfUniqueUsers()
    {
        var logEntries = new List<LogEntry>
        {
            new() { User = "ABC123"},
            new() { User = "ABC123"},
            new() { User = "User999"},
            new() { User = "User999"},
            new() { User = "Test"},
            new() { User = ""}
        };

        var result = LogStatistics.CountUniqueItems(logEntries, log => log.User);
        Assert.Equal(4, result);
    }

    [Fact]
    public void CountUniqueItems_ShouldExcludeMissingIPsAndReturnUniqueIPCount()
    {
        var logEntries = new List<LogEntry>
        {
            new() { IPAddress = "111.11.111.11"},
            new() { IPAddress = "111.11.111.11"},
            new() { IPAddress = "222.22.222.22"},
            new() { IPAddress = ""}
        };

        var result = LogStatistics.CountUniqueItems(logEntries, log => log.IPAddress, filterMissing: true);
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetTopItems_ShouldReturnTopNURLs()
    {
        var logEntries = new List<LogEntry>
        {
            new() { Url = "/home"},
            new() { Url = "/home"},
            new() { Url = "/about"},
            new() { Url = "/about"},
            new() { Url = "/contact"}
        };

        var result = LogStatistics.GetTopItems(logEntries, log => log.Url, 2);

        Assert.Equal(2, result.Count);
        Assert.Equal(2, result["/home"]);
        Assert.Equal(2, result["/about"]);
    }

    [Fact]
    public void GetTopItems_ShouldReturnTopNIPs()
    {
        var logEntries = new List<LogEntry>
        {
            new() { IPAddress = "111.11.111.11"},
            new() { IPAddress = "111.11.111.11"},
            new() { IPAddress = "222.22.222.22"},
            new() { IPAddress = "222.22.222.22"},
            new() { IPAddress = "222.22.222.22"},
            new() { IPAddress = "333.33.333.33"}
        };

        var result = LogStatistics.GetTopItems(logEntries, log => log.IPAddress, 2);

        Assert.Equal(2, result.Count);
        Assert.Equal(2, result["111.11.111.11"]);
        Assert.Equal(3, result["222.22.222.22"]);
    }

    [Fact]
    public void GetTopItems_ShouldReturnTopNActiveUsers()
    {
        var logEntries = new List<LogEntry>
        {
            new() { User = "ABC123"},
            new() { User = "ABC123"},
            new() { User = "User999"},
            new() { User = "User999"},
            new() { User = "Test"}
        };

        var result = LogStatistics.GetTopItems(logEntries, log => log.User, 2);

        Assert.Equal(2, result.Count);
        Assert.Equal(2, result["ABC123"]);
        Assert.Equal(2, result["User999"]);
    }

    [Fact]
    public void GetTopItems_ShouldReturnAllUsersWhereNHigherThanNoOfUniqueUsers()
    {
        var logEntries = new List<LogEntry>
        {
            new() { User = "ABC123"},
            new() { User = "ABC123"},
            new() { User = "User999"},
            new() { User = "User999"},
            new() { User = "Test"}
        };

        var result = LogStatistics.GetTopItems(logEntries, log => log.User, 4);

        Assert.Equal(3, result.Count);
        Assert.Equal(2, result["ABC123"]);
        Assert.Equal(2, result["User999"]);
        Assert.Equal(1, result["Test"]);
    }

    [Fact]
    public void GetTopItems_ShouldReturnEmptyResultIfRequiredFieldMissing()
    {
        var logEntries = new List<LogEntry>{
            new() { User = "ABC123"},
            new() { User = "ABC123"},
            new() { User = "User999"},
            new() { User = "User999"},
            new() { User = "Test"}
        };

        var result = LogStatistics.GetTopItems(logEntries, log => log.Url, 4, filterMissing: true);

        Assert.Empty(result);
    }

    [Fact]
    public void GetTopItems_ShouldReturnTopActiveUsersIncludingTies()
    {
        var logEntries = new List<LogEntry>
        {
            new() { User = "ABC123"},
            new() { User = "ABC123"},
            new() { User = "User999"},
            new() { User = "User999"},
            new() { User = ""},
            new() { User = ""}
        };

        var result = LogStatistics.GetTopItems(logEntries, log => log.User, 1, filterMissing: false, includeTies: true);

        Assert.Equal(3, result.Count);
        Assert.Equal(2, result["ABC123"]);
        Assert.Equal(2, result["User999"]);
        Assert.Equal(2, result[""]);
    }

    [Fact]
    public void GetTopItems_ShouldReturnTopActiveUsersIncludingTiesExcludingMissingUsers()
    {
        var logEntries = new List<LogEntry>
        {
            new() { User = "ABC123"},
            new() { User = "ABC123"},
            new() { User = "User999"},
            new() { User = "User999"},
            new() { User = ""},
            new() { User = ""}
        };

        var result = LogStatistics.GetTopItems(logEntries, log => log.User, 1, filterMissing: true, includeTies: true);

        Assert.Equal(2, result.Count);
        Assert.Equal(2, result["ABC123"]);
        Assert.Equal(2, result["User999"]);
    }
}