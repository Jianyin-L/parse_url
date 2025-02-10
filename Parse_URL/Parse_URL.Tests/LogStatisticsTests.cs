using Parse_URL.Model;
using Parse_URL.Utilities;

namespace Parse_URL.Tests;

public class LogStatisticsTests
{
    [Fact]
    public void CountUniqueItems_ShouldReturnNumberOfUniqueIPs()
    {
        var logEntries = new List<LogEntry>
        {
            new() { IPAddress = "111.11.111.11"},
            new() { IPAddress = "111.11.111.11"},
            new() { IPAddress = "222.22.222.22"}
        };

        var result = LogStatistics.CountUniqueItems(logEntries, log => log.IPAddress);
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetTopItems_ShouldReturnTopURLs()
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
    public void GetTopItems_ShouldReturnTopIPs()
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
    public void CountUniqueItems_ShouldReturnNumberOfUniqueItems()
    {
        var logEntries = new List<LogEntry>
        {
            new() { Url = "/home"},
            new() { Url = "/home"},
            new() { Url = "/about"},
            new() { Url = "/about"},
            new() { Url = "/contact"}
        };

        var result = LogStatistics.CountUniqueItems(logEntries, log => log.Url);
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetTopItems_ShouldReturnTopNItems()
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
    public void GetTopItems_ShouldReturnTopItemsWhereNHigherThanNoOfEntries()
    {
        var logEntries = new List<LogEntry>
        {
            new() { Url = "/home"},
            new() { Url = "/home"},
            new() { Url = "/about"},
            new() { Url = "/about"},
            new() { Url = "/contact"}
        };

        var result = LogStatistics.GetTopItems(logEntries, log => log.Url, 4);

        Assert.Equal(3, result.Count);
        Assert.Equal(2, result["/home"]);
        Assert.Equal(2, result["/about"]);
        Assert.Equal(1, result["/contact"]);
    }

    [Fact]
    public void GetTopItems_ShouldReturnTopNItemsIncludingTies()
    {
        var logEntries = new List<LogEntry>
        {
            new() { Url = "/home"},
            new() { Url = "/home"},
            new() { Url = "/about"},
            new() { Url = "/about"},
            new() { Url = "/contact"}
        };

        var result = LogStatistics.GetTopItemsIncludingTies(logEntries, log => log.Url, 1);
        Assert.Equal(2, result.Count);
        Assert.Equal(2, result["/home"]);
        Assert.Equal(2, result["/about"]);
    }

    //TODO: handle empty fields
}