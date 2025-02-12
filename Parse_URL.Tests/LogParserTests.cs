using Parse_URL.Models;
using Parse_URL.Utilities;
using System.Globalization;

namespace Parse_URL.Tests;

public class LogParserTests
{
    [Fact]
    public void ParseLogFile_ShouldParseValidLogEntries()
    {
        // Arrange
        var logFilePath = CreateTestLogFile("177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"");

        // Act
        var result = LogParser.ParseLogFile(logFilePath);

        // Assert
        Assert.Single(result);

        Assert.Equal("177.71.128.21", result[0].IPAddress);
        Assert.Equal("-", result[0].User);
        Assert.Equal("10/Jul/2018:22:21:28 +02:00", result[0].Timestamp.ToString("dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture));
        Assert.Equal("GET", result[0].HttpMethod);
        Assert.Equal("/home", result[0].Url);
        Assert.Equal(200, result[0].StatusCode);
        Assert.Equal(3574, result[0].ResponseSize);
        Assert.Equal("Mozilla/5.0", result[0].UserAgent);
    }

    [Fact]
    public void ParseLogFile_ShouldParseLogEntriesWithValidUser()
    {
        // Arrange
        var logFilePath = CreateTestLogFile(
            "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // User is unknown
            "177.71.128.21 - admin [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // User is an admin
            "177.71.128.21 - Jen123 [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"" // User is Jen123 
        );

        // Act
        var result = LogParser.ParseLogFile(logFilePath);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("-", result[0].User);
        Assert.Equal("admin", result[1].User);
        Assert.Equal("Jen123", result[2].User);
    }

    [Fact]
    public void ParseLogFile_ShouldParseLogEntriesWithDifferentTimeZones()
    {
        // Arrange
        var logFilePath = CreateTestLogFile(
            "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"",
            "177.71.128.21 - - [10/Jul/2018:22:21:28 -0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"",
            "177.71.128.21 - - [10/Jul/2018:22:21:28 +0000] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\""
        );

        // Act
        var result = LogParser.ParseLogFile(logFilePath);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("10/Jul/2018:22:21:28 +02:00", result[0].Timestamp.ToString("dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture));
        Assert.Equal("10/Jul/2018:22:21:28 -02:00", result[1].Timestamp.ToString("dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture));
        Assert.Equal("10/Jul/2018:22:21:28 +00:00", result[2].Timestamp.ToString("dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ParseLogFile_ShouldParseLogEntriesWithValidHttpMethod()
    {
        // Arrange
        var logFilePath = CreateTestLogFile(
            "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"get /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // GET method but in lowercase
            "177.71.128.22 - - [10/Jul/2018:22:21:28 +0200] \"PUT /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"" // PUT method
        );

        // Act
        var result = LogParser.ParseLogFile(logFilePath);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("177.71.128.21", result[0].IPAddress);
        Assert.Equal("177.71.128.22", result[1].IPAddress);
    }

    [Fact]
    public void ParseLogFile_ShouldParseLogEntriesWithSpecialCharactersInUrl()
    {
        // Arrange
        var logFilePath = CreateTestLogFile(
            "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home/search?q=test HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"" // special characters in URL
        );

        // Act
        var result = LogParser.ParseLogFile(logFilePath);

        // Assert
        Assert.Single(result);
        Assert.Equal("/home/search?q=test", result[0].Url);
    }

    [Fact]
    public void ParseLogFile_ShouldParseLogEntriesWithValidStatusCode()
    {
        // Arrange
        var logFilePath = CreateTestLogFile(
            "222.22.222.22 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 404 3574 \"-\" \"Mozilla/5.0\"" // status code 404
        );

        // Act
        var result = LogParser.ParseLogFile(logFilePath);

        // Assert
        Assert.Single(result);
        Assert.Equal(404, result[0].StatusCode);
    }

    [Fact]
    public void ParseLogFile_ShouldParseLogEntrieaWithMissingFields()
    {
        // Arrange
        var logFilePath = CreateTestLogFile(
            "111.11.111.11 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // user is missing
            "222.22.222.22 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 - \"-\" \"Mozilla/5.0\"", // response size is missing
            "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"-\"" // user agent is missing
        );

        // Act
        var result = LogParser.ParseLogFile(logFilePath);

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void ParseLogFile_ShouldFailGracefullyForInvalidLogEntries()
    {
        // Arrange
        var logFilePath = CreateTestLogFile(
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua", // Invalid log entry
            "XYZ.XY.XYZ.XY - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // Invalid IP Address
            "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"XYZ /home HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0\"", // Invalid HTTP method
            "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 9999 3574 \"-\" \"Mozilla/5.0\"", // Invalid status code
            "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /home HTTP/1.1\" 200 -3574 \"-\" \"Mozilla/5.0\"" // Negative response size
        );

        // Act
        var result = LogParser.ParseLogFile(logFilePath);

        // Assert
        Assert.Empty(result);
    }

    private string CreateTestLogFile(params string[] logEntries)
    {
        var logFilePath = Path.GetTempFileName();
        File.WriteAllLines(logFilePath, logEntries);
        return logFilePath;
    }
}