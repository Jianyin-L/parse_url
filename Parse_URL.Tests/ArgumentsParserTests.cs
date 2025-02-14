using Parse_URL.Utilities;

namespace Parse_URL.Tests;

public class ArgumentsParserTests
{
    [Fact]
    public void ParseArguments_ShouldReturnDefaultValues_WhenNoArgumentsProvided()
    {
        var args = Array.Empty<string>();
        var (filePath, urls, ips, filterMissing, includeTies) = ArgumentsParser.ParseArguments(args);

        Assert.Contains("example.log", filePath);
        Assert.Equal(3, urls);
        Assert.Equal(3, ips);
        Assert.False(filterMissing);
        Assert.False(includeTies);
    }

    //[Fact]
    //public void ParseArguments_ShouldParseFilePath_WhenProvided()
    //{
    //    var args = new[] { "file=C:\\logs\\access.log" };
    //    var (filePath, _, _, _, _) = ArgumentsParser.ParseArguments(args);

    //    Assert.Equal("C:\\logs\\access.log", filePath);
    //}

    [Fact]
    public void ParseArguments_ShouldReturnDefaultFilePath_WhenProvidedNotValid()
    {
        var args = new[] { "file=abcdefg.log" };
        var (filePath, _, _, _, _) = ArgumentsParser.ParseArguments(args);

        Assert.Contains("example.log", filePath);
    }

    [Theory]
    [InlineData("urls=10", 10)]
    [InlineData("urls=-5", 5)]
    [InlineData("urls=1.2", 1)]
    [InlineData("urls=1.6", 2)]
    public void ParseArguments_ShouldParseNumberOfUrls_WhenValidIntegerProvided(string input, int expected)
    {
        var args = new string[] { input };

        var (_, urls, _, _, _) = ArgumentsParser.ParseArguments(args);

        Assert.Equal(expected, urls);
    }

    [Theory]
    [InlineData("urls=abc")]
    [InlineData("urls=null")]
    [InlineData("urls= ")]
    [InlineData("urls==")]
    [InlineData("urls=...")]
    public void ParseArguments_ShouldReturnDefaultUrls_WhenInvalidValuesGiven(string input)
    {
        var args = new string[] { input };
        var (_, urls, _, _, _) = ArgumentsParser.ParseArguments(args);

        Assert.Equal(3, urls);
    }

    [Theory]
    [InlineData("ips=10", 10)]
    [InlineData("ips=-5", 5)]
    [InlineData("ips=1.2", 1)]
    [InlineData("ips=1.6", 2)]
    public void ParseArguments_ShouldParseNumberOfIps_WhenValidIntegerProvided(string input, int expected)
    {
        var args = new string[] { input };

        var (_, _, ips, _, _) = ArgumentsParser.ParseArguments(args);

        Assert.Equal(expected, ips);
    }

    [Theory]
    [InlineData("urls=abc")]
    [InlineData("urls=null")]
    [InlineData("urls= ")]
    [InlineData("urls==")]
    [InlineData("urls=...")]
    public void ParseArguments_ShouldReturnDefaultIps_WhenInvalidValuesIsGiven(string input)
    {
        var args = new[] { input };
        var (_, _, ips, _, _) = ArgumentsParser.ParseArguments(args);

        Assert.Equal(3, ips);
    }

    [Theory]
    [InlineData("filtermissing=true", "includeties=false", true, false)]
    [InlineData("filtermissing=t", "includeties=f", true, false)]
    [InlineData("filtermissing=TRUE", "includeties=FALSE", true, false)]
    [InlineData("filtermissing=0", "includeties=1", false, true)]
    [InlineData("filtermissing=no", "includeties=yes", false, true)]
    [InlineData("filtermissing=N", "includeties=y", false, true)]
    public void ParseArguments_ShouldParseBooleanFlags_WhenValidValueIsGiven(string missing, string ties, bool expectedMissing, bool expectedTies)
    {
        var args = new[] { missing, ties };
        var (_, _, _, filterMissing, includeTies) = ArgumentsParser.ParseArguments(args);

        Assert.Equal(expectedMissing, filterMissing);
        Assert.Equal(expectedTies, includeTies);
    }

    [Theory]
    [InlineData("filtermissing=abc", "includeties=123", false, false)]
    [InlineData("filtermissing=  ", "includeties=!!", false, false)]
    public void ParseArguments_ShouldReturnDefaultBooleanFlags_WhenInvalidValueIsGiven(string missing, string ties, bool expectedMissing, bool expectedTies)
    {
        var args = new[] { missing, ties };
        var (_, _, _, filterMissing, includeTies) = ArgumentsParser.ParseArguments(args);

        Assert.Equal(expectedMissing, filterMissing);
        Assert.Equal(expectedTies, includeTies);
    }
}
