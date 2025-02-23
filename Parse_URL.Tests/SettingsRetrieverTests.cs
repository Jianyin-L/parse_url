using Parse_URL.Configs;
using Microsoft.Extensions.Configuration;

namespace Parse_URL.Tests;

public class SettingsRetrieverTests
{
    private static IConfiguration GetConfig()
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
    public void SettingsRetriever_ShouldReturnDefaultValues_WhenNoArgumentsProvided()
    {
        var config = GetConfig();
        var settings = SettingsRetriever.RetrieveFromConfig(config);

        var args = Array.Empty<string>();
        var (filePath, urls, ips, filterMissing, includeTies) = SettingsRetriever.RetrieveArguments(args, settings);

        Assert.Contains("example.log", filePath);
        Assert.Equal(3, urls);
        Assert.Equal(3, ips);
        Assert.False(filterMissing);
        Assert.False(includeTies);
    }

    [Fact]
    public void SettingsRetriever_ShouldReturnDefaultValues_WhenInvalidArgumentsProvided()
    {
        var config = GetConfig();
        var settings = SettingsRetriever.RetrieveFromConfig(config);
        var args = new[] {
            "XYZ=abcdefg.log ABC=10 Random=10 YYY=true ZZZ=true",
        };

        var (filePath, urls, ips, filterMissing, includeTies) = SettingsRetriever.RetrieveArguments(args, settings);

        Assert.Contains("example.log", filePath);
        Assert.Equal(3, urls);
        Assert.Equal(3, ips);
        Assert.False(filterMissing);
        Assert.False(includeTies);
    }

    [Theory]
    [InlineData("file=abcdefg.log")]
    [InlineData("file= ")]
    public void SettingsRetriever_ShouldReturnDefaultFilePath_WhenInValidFileIsGiven(string file)
    {
        var config = GetConfig();
        var settings = SettingsRetriever.RetrieveFromConfig(config);
        var args = new[] { file };

        var (filePath, _, _, _, _) = SettingsRetriever.RetrieveArguments(args, settings);

        Assert.Contains("example.log", filePath);
    }

    [Theory]
    [InlineData("urls=10", 10)]
    [InlineData("urls=0", 3)]
    [InlineData("urls=-5", 5)]
    [InlineData("urls=1.2", 1)]
    [InlineData("urls=1.6", 2)]
    public void SettingsRetriever_ShouldParseNumberOfUrls_WhenValidIntegerIsGiven(string input, int expected)
    {
        var config = GetConfig();
        var settings = SettingsRetriever.RetrieveFromConfig(config);
        var args = new string[] { input };

        var (_, urls, _, _, _) = SettingsRetriever.RetrieveArguments(args, settings);

        Assert.Equal(expected, urls);
    }

    [Theory]
    [InlineData("urls=abc")]
    [InlineData("urls=null")]
    [InlineData("urls= ")]
    [InlineData("urls==")]
    [InlineData("urls=...")]
    public void SettingsRetriever_ShouldReturnDefaultUrls_WhenInvalidValuesIsGiven(string input)
    {
        var config = GetConfig();
        var settings = SettingsRetriever.RetrieveFromConfig(config);
        var args = new string[] { input };

        var (_, urls, _, _, _) = SettingsRetriever.RetrieveArguments(args, settings);

        Assert.Equal(3, urls);
    }

    [Theory]
    [InlineData("ips=10", 10)]
    [InlineData("ips=0", 3)]
    [InlineData("ips=-5", 5)]
    [InlineData("ips=1.2", 1)]
    [InlineData("ips=1.6", 2)]
    [InlineData("ips=10 ", 10)]
    public void SettingsRetriever_ShouldParseNumberOfIps_WhenValidIntegerIsGiven(string input, int expected)
    {
        var config = GetConfig();
        var settings = SettingsRetriever.RetrieveFromConfig(config);
        var args = new string[] { input };

        var (_, _, ips, _, _) = SettingsRetriever.RetrieveArguments(args, settings);

        Assert.Equal(expected, ips);
    }

    [Theory]
    [InlineData("urls=abc")]
    [InlineData("urls=null")]
    [InlineData("urls= ")]
    [InlineData("urls==")]
    [InlineData("urls=...")]
    public void SettingsRetriever_ShouldReturnDefaultIps_WhenInvalidValuesIsGiven(string input)
    {
        var config = GetConfig();
        var settings = SettingsRetriever.RetrieveFromConfig(config);
        var args = new[] { input };

        var (_, _, ips, _, _) = SettingsRetriever.RetrieveArguments(args, settings);

        Assert.Equal(3, ips);
    }

    [Theory]
    [InlineData("filtermissing=true", "includeties=false", true, false)]
    [InlineData("filtermissing=t", "includeties=f", true, false)]
    [InlineData("filtermissing=TRUE", "includeties=FALSE", true, false)]
    [InlineData("filtermissing=0", "includeties=1", false, true)]
    [InlineData("filtermissing=no", "includeties=yes", false, true)]
    [InlineData("filtermissing=N", "includeties=y", false, true)]
    public void SettingsRetriever_ShouldParseBooleanFlags_WhenValidValueIsGiven(string missing, string ties, bool expectedMissing, bool expectedTies)
    {
        var config = GetConfig();
        var settings = SettingsRetriever.RetrieveFromConfig(config);
        var args = new[] { missing, ties };

        var (_, _, _, filterMissing, includeTies) = SettingsRetriever.RetrieveArguments(args, settings);

        Assert.Equal(expectedMissing, filterMissing);
        Assert.Equal(expectedTies, includeTies);
    }

    [Theory]
    [InlineData("filtermissing=abc", "includeties=123", false, false)]
    [InlineData("filtermissing=  ", "includeties=!!", false, false)]
    public void SettingsRetriever_ShouldReturnDefaultBooleanFlags_WhenInvalidValueIsGiven(string missing, string ties, bool expectedMissing, bool expectedTies)
    {
        var config = GetConfig();
        var settings = SettingsRetriever.RetrieveFromConfig(config);
        var args = new[] { missing, ties };

        var (_, _, _, filterMissing, includeTies) = SettingsRetriever.RetrieveArguments(args, settings);

        Assert.Equal(expectedMissing, filterMissing);
        Assert.Equal(expectedTies, includeTies);
    }
}
