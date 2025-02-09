using Parse_URL.Model;
using Parse_URL.Utilities;
using System.Globalization;

namespace Parse_URL.Tests;

public class LogParserTests
{
    //[Fact]
    //public void ParseLogFile_ShouldCountUniqueIPs()
    //{
    //    var parser = new LogParser();
    //    string logFilePath = "test_log.txt";

    //    File.WriteAllLines(logFilePath, new[]
    //    {
    //    "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"UserAgent1\"",
    //    "168.41.191.40 - - [10/Jul/2018:22:21:29 +0200] \"GET /about HTTP/1.1\" 200 3574 \"-\" \"UserAgent2\"",
    //    "177.71.128.21 - - [10/Jul/2018:22:21:30 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"UserAgent3\""
    //    });

    //    var result = parser.ParseLogFile(logFilePath);

    //    Assert.Equal(2, result.UniqueIPs);
    //}

    //[Fact]
    //public void ParseLogFile_ShouldReturnTopUrls()
    //{
    //    var parser = new LogParser();
    //    string logFilePath = "test_log.txt";

    //    File.WriteAllLines(logFilePath, new[]
    //    {
    //    "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"UserAgent1\"",
    //    "168.41.191.40 - - [10/Jul/2018:22:21:29 +0200] \"GET /about HTTP/1.1\" 200 3574 \"-\" \"UserAgent2\"",
    //    "177.71.128.21 - - [10/Jul/2018:22:21:30 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"UserAgent3\"",
    //    "168.41.191.40 - - [10/Jul/2018:22:21:31 +0200] \"GET /contact HTTP/1.1\" 200 3574 \"-\" \"UserAgent4\"",
    //    "168.41.191.40 - - [10/Jul/2018:22:21:32 +0200] \"GET /about HTTP/1.1\" 200 3574 \"-\" \"UserAgent5\""
    //    });

    //    var result = parser.ParseLogFile(logFilePath);

    //    Assert.Equal(3, result.TopUrls.Count);
    //    Assert.Equal("/home", result.TopUrls.Keys.First());
    //}

    //[Fact]
    //public void ParseLogFile_ShouldReturnTopIPs()
    //{
    //    var parser = new LogParser();
    //    string logFilePath = "test_log.txt";

    //    File.WriteAllLines(logFilePath, new[]
    //    {
    //    "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"UserAgent1\"",
    //    "168.41.191.40 - - [10/Jul/2018:22:21:29 +0200] \"GET /about HTTP/1.1\" 200 3574 \"-\" \"UserAgent2\"",
    //    "177.71.128.21 - - [10/Jul/2018:22:21:30 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"UserAgent3\"",
    //    "168.41.191.40 - - [10/Jul/2018:22:21:31 +0200] \"GET /contact HTTP/1.1\" 200 3574 \"-\" \"UserAgent4\"",
    //    "168.41.191.40 - - [10/Jul/2018:22:21:32 +0200] \"GET /about HTTP/1.1\" 200 3574 \"-\" \"UserAgent5\""
    //});

    //    var result = parser.ParseLogFile(logFilePath);

    //    Assert.Equal(2, result.TopIPs.Count);
    //    Assert.Equal("168.41.191.40", result.TopIPs.Keys.First());
    //}

    [Fact]
    public void ParseLogFile_ShouldParseLogEntries()
    {
        // Arrange
        string logFilePath = "test_log.txt";

        File.WriteAllLines(logFilePath, new[]
        {
            "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // valid log entry
            
            "177.71.128.XY - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // invalid IP
            "  - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // IP missing

            "177.71.128.21 - admin [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // admin user
            "177.71.128.21 - 123abc [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // use is 123abc
            
            "111.11.111.11 - 123abc [10/Jul/2018:22:21:28 +0200] \"POST /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // POST method
            "222.22.222.22 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 400 3574 \"-\" \"Mozilla/5.0\"", // status code 400
        });

        // Act
        var result = LogParser.ParseLogFile(logFilePath);

        // Assert
        Assert.Equal(5, result.Count);

        Assert.Equal("177.71.128.21", result[0].IPAddress);
        Assert.Equal("-", result[0].User);
        Assert.Equal("10/Jul/2018:22:21:28 +02:00", result[0].Timestamp.ToString("dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture));
        Assert.Equal("GET", result[0].HttpMethod);
        Assert.Equal("/home", result[0].Url);
        Assert.Equal(200, result[0].StatusCode);
        Assert.Equal(3574, result[0].ResponseSize);
        Assert.Equal("Mozilla/5.0", result[0].UserAgent);

        Assert.Equal("admin", result[1].User);
        Assert.Equal("123abc", result[2].User);
        Assert.Equal("111.11.111.11", result[3].IPAddress);
        Assert.Equal("222.22.222.22", result[4].IPAddress);
    }

    [Fact]
    public void CountUniqueItems_ShouldReturnUniqueIPs()
    {
        var logEntries = new List<LogEntry>
        {
            new() { IPAddress = "123.12.123.12"},
            new() { IPAddress = "123.12.123.12"},
            new() { IPAddress = "222.12.123.12"}
        };

        int result = LogParser.CountUniqueItems(logEntries, log => log.IPAddress);
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetTopItems_ShouldReturnTopItems()
    {
        var logEntries = new List<LogEntry>
        {
            new() { Url = "/home"},
            new() { Url = "/home"},
            new() { Url = "/about"},
            new() { Url = "/about"},
            new() { Url = "/contact"}
        };

        var result = LogParser.GetTopItems(logEntries, log => log.Url, 2);

        Assert.Equal(2, result.Count);
        Assert.Equal(2, result["/home"]);
        Assert.Equal(2, result["/about"]);
    }

    [Fact]
    public void GetTopItems_ShouldReturnTopItemsIncludingTies()
    {
        var logEntries = new List<LogEntry>
        {
            new() { Url = "/home"},
            new() { Url = "/home"},
            new() { Url = "/about"},
            new() { Url = "/about"},
            new() { Url = "/contact"}
        };

        var result = LogParser.GetTopItemsIncludingTies(logEntries, log => log.Url, 1);
        Assert.Equal(2, result.Count);
        Assert.Equal(2, result["/home"]);
        Assert.Equal(2, result["/about"]);
    }
}