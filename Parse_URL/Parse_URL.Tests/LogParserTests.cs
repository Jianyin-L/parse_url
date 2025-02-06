using Parse_URL.Model;
using Parse_URL.Utilities;

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
    public void ParseLogFile_ShouldReturnLogEntries()
    {
        var parser = new LogParser();
        string logFilePath = "test_log.txt";

        File.WriteAllLines(logFilePath, new[]
        {
            "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"",
            "168.41.191.40 - - [10/Jul/2018:22:21:29 +0200] \"POST /api/data HTTP/1.1\" 201 512 \"-\" \"curl/7.64.1\""
        });

        List<LogEntry> result = LogParser.ParseLogFile(logFilePath);

        Assert.Equal(2, result.Count);
        Assert.Equal("177.71.128.21", result[0].IPAddress);
        Assert.Equal("/home", result[0].Url);
        Assert.Equal(200, result[0].StatusCode);
        Assert.Equal("Mozilla/5.0", result[0].UserAgent);
        Assert.Equal("168.41.191.40", result[1].IPAddress);
        Assert.Equal("/api/data", result[1].Url);
        Assert.Equal(201, result[1].StatusCode);
        Assert.Equal("curl/7.64.1", result[1].UserAgent);
    }

    [Fact]
    public void CountUniqueItems_ShouldReturnUniqueIPs()
    {
        var logEntries = new List<LogEntry>
        {
            new() { IPAddress = "123.12.123.12"},
            new() { IPAddress = "123.12.123.12"},
            new() { IPAddress = "333.12.123.12"}
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
}