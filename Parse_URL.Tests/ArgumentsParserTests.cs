using Parse_URL.Configs;
using Parse_URL.Utilities;
using Microsoft.Extensions.Configuration;

namespace Parse_URL.Tests;

public class ArgumentsParserTests
{
    private static IConfiguration GetMockConfig()
    {
        var configValues = new Dictionary<string, string?>
        {
            { "Defaults:FilePath", "./Data/example.log" },
            { "Defaults:NumberOfUrls", "3" },
            { "Defaults:NumberOfIps", "3" },
            { "Defaults:FilterMissingField", "false" },
            { "Defaults:IncludeTies", "false" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        return configuration;
    }

    [Fact]
    public void ParseArguments_ShouldReturnDefaultValues_WhenNoArgumentsProvided()
    {
        var config = GetMockConfig();
        var defaultSettings = new AppSettings();
        config.GetSection("Defaults").Bind(defaultSettings);

        var args = Array.Empty<string>();
        var (filePath, urls, ips, filterMissing, includeTies) = ArgParser.ParseArguments(args, defaultSettings);

        Assert.Contains("example.log", filePath);
        Assert.Equal(3, urls);
        Assert.Equal(3, ips);
        Assert.False(filterMissing);
        Assert.False(includeTies);
    }

    [Fact]
    public void ParseArguments_ShouldReturnDefaultValues_WhenInvalidArgumentsProvided()
    {
        var config = GetMockConfig();
        var defaultSettings = new AppSettings();
        config.GetSection("Defaults").Bind(defaultSettings);
        var args = new[] {
            "XYZ=abcdefg.log ABC=10 Random=10 YYY=true ZZZ=true",
        };

        var (filePath, urls, ips, filterMissing, includeTies) = ArgParser.ParseArguments(args, defaultSettings);

        Assert.Contains("example.log", filePath);
        Assert.Equal(3, urls);
        Assert.Equal(3, ips);
        Assert.False(filterMissing);
        Assert.False(includeTies);
    }

    [Theory]
    [InlineData("file=abcdefg.log")]
    [InlineData("file= ")]
    public void ParseArguments_ShouldReturnDefaultFilePath_WhenInValidFileIsGiven(string file)
    {
        var config = GetMockConfig();
        var defaultSettings = new AppSettings();
        config.GetSection("Defaults").Bind(defaultSettings);
        var args = new[] { file };

        var (filePath, _, _, _, _) = ArgParser.ParseArguments(args, defaultSettings);

        Assert.Contains("example.log", filePath);
    }

    [Theory]
    [InlineData("urls=10", 10)]
    [InlineData("urls=0", 3)]
    [InlineData("urls=-5", 5)]
    [InlineData("urls=1.2", 1)]
    [InlineData("urls=1.6", 2)]
    public void ParseArguments_ShouldParseNumberOfUrls_WhenValidIntegerIsGiven(string input, int expected)
    {
        var config = GetMockConfig();
        var defaultSettings = new AppSettings();
        config.GetSection("Defaults").Bind(defaultSettings);
        var args = new string[] { input };

        var (_, urls, _, _, _) = ArgParser.ParseArguments(args, defaultSettings);

        Assert.Equal(expected, urls);
    }

    [Theory]
    [InlineData("urls=abc")]
    [InlineData("urls=null")]
    [InlineData("urls= ")]
    [InlineData("urls==")]
    [InlineData("urls=...")]
    public void ParseArguments_ShouldReturnDefaultUrls_WhenInvalidValuesIsGiven(string input)
    {
        var config = GetMockConfig();
        var defaultSettings = new AppSettings();
        config.GetSection("Defaults").Bind(defaultSettings);
        var args = new string[] { input };

        var (_, urls, _, _, _) = ArgParser.ParseArguments(args, defaultSettings);

        Assert.Equal(3, urls);
    }

    [Theory]
    [InlineData("ips=10", 10)]
    [InlineData("ips=0", 3)]
    [InlineData("ips=-5", 5)]
    [InlineData("ips=1.2", 1)]
    [InlineData("ips=1.6", 2)]
    [InlineData("ips=10 ", 10)]
    public void ParseArguments_ShouldParseNumberOfIps_WhenValidIntegerIsGiven(string input, int expected)
    {
        var config = GetMockConfig();
        var defaultSettings = new AppSettings();
        config.GetSection("Defaults").Bind(defaultSettings);
        var args = new string[] { input };

        var (_, _, ips, _, _) = ArgParser.ParseArguments(args, defaultSettings);

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
        var config = GetMockConfig();
        var defaultSettings = new AppSettings();
        config.GetSection("Defaults").Bind(defaultSettings);
        var args = new[] { input };

        var (_, _, ips, _, _) = ArgParser.ParseArguments(args, defaultSettings);

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
        var config = GetMockConfig();
        var defaultSettings = new AppSettings();
        config.GetSection("Defaults").Bind(defaultSettings);
        var args = new[] { missing, ties };

        var (_, _, _, filterMissing, includeTies) = ArgParser.ParseArguments(args, defaultSettings);

        Assert.Equal(expectedMissing, filterMissing);
        Assert.Equal(expectedTies, includeTies);
    }

    [Theory]
    [InlineData("filtermissing=abc", "includeties=123", false, false)]
    [InlineData("filtermissing=  ", "includeties=!!", false, false)]
    public void ParseArguments_ShouldReturnDefaultBooleanFlags_WhenInvalidValueIsGiven(string missing, string ties, bool expectedMissing, bool expectedTies)
    {
        var config = GetMockConfig();
        var defaultSettings = new AppSettings();
        config.GetSection("Defaults").Bind(defaultSettings);
        var args = new[] { missing, ties };

        var (_, _, _, filterMissing, includeTies) = ArgParser.ParseArguments(args, defaultSettings);

        Assert.Equal(expectedMissing, filterMissing);
        Assert.Equal(expectedTies, includeTies);
    }
}
