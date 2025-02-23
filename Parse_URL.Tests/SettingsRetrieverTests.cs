using Parse_URL.Configs;
using Microsoft.Extensions.Configuration;

namespace Parse_URL.Tests;

public class SettingsRetrieverTests
{
    [Fact]
    public void RetrieveConfigs_ShouldReturnConfig_WhenValidConfigIsProvided()
    {
        var defaults = new Dictionary<string, string?>
        {
            { "Defaults:FilePath", "./Data/example.log" },
            { "Defaults:NumberOfUrls", "9" },
            { "Defaults:NumberOfIps", "9" },
            { "Defaults:FilterMissingField", "true" },
            { "Defaults:IncludeTies", "false" }
        };
        var config = GetConfig(defaults);

        var (FilePath, NumberOfUrls, NumberOfIps, FilterMissingField, IncludeTies) = SettingsRetriever.RetrieveConfigs(config);

        Assert.Contains("example.log", FilePath);
        Assert.Equal(9, NumberOfUrls);
        Assert.Equal(9, NumberOfIps);
        Assert.True(FilterMissingField);
        Assert.False(IncludeTies);
    }

    [Fact]
    public void RetrieveConfigs_ShouldFallbackToDefaults_WhenInvalidConfigValueIsProvided()
    {
        var defaults = new Dictionary<string, string?>
        {
            { "Defaults:FilePath", "abc.log" },
            { "Defaults:NumberOfUrls", "" },
            { "Defaults:NumberOfIps", null },
            { "Defaults:FilterMissingField", "abc" },
            { "Defaults:IncludeTies", "9" }
        };
        var config = GetConfig(defaults);

        var (FilePath, NumberOfUrls, NumberOfIps, FilterMissingField, IncludeTies) = SettingsRetriever.RetrieveConfigs(config);

        Assert.Contains(DefaultSettings.FilePath, FilePath);
        Assert.Equal(DefaultSettings.NumberOfUrls, NumberOfUrls);
        Assert.Equal(DefaultSettings.NumberOfIps, NumberOfIps);
        Assert.Equal(DefaultSettings.FilterMissingField, FilterMissingField);
        Assert.Equal(DefaultSettings.IncludeTies, IncludeTies);
    }

    [Fact]
    public void RetrieveConfigs_ShouldReturnDefaults_WhenInvalidConfigArgumentIsProvided()
    {
        var defaults = new Dictionary<string, string?>
        {
            { "FilePath", "./Data/example.log" },
            { "Defaults:Urls", "3" },
            { "XYZ:NumberOfIps", "3" },
            { "Defaults:ABC:FilterMissingField", "true" },
            { "ABCDEF", "false" }
        };
        var config = GetConfig(defaults);

        var (FilePath, NumberOfUrls, NumberOfIps, FilterMissingField, IncludeTies) = SettingsRetriever.RetrieveConfigs(config);

        Assert.Contains(DefaultSettings.FilePath, FilePath);
        Assert.Equal(DefaultSettings.NumberOfUrls, NumberOfUrls);
        Assert.Equal(DefaultSettings.NumberOfIps, NumberOfIps);
        Assert.Equal(DefaultSettings.FilterMissingField, FilterMissingField);
        Assert.Equal(DefaultSettings.IncludeTies, IncludeTies);
    }

    [Fact]
    public void RetrieveInputs_ShouldReturnDefaultValues_WhenNoArgumentsProvided()
    {
        var settings = GetDefaultConfigValues();
        var args = Array.Empty<string>();

        var (filePath, urls, ips, filterMissing, includeTies) = SettingsRetriever.RetrieveInputs(args, settings);

        Assert.Contains("example.log", filePath);
        Assert.Equal(3, urls);
        Assert.Equal(3, ips);
        Assert.False(filterMissing);
        Assert.False(includeTies);
    }

    [Fact]
    public void RetrieveInputs_ShouldReturnDefaultValues_WhenInvalidArgumentsProvided()
    {
        var settings = GetDefaultConfigValues();
        var args = new[] {
            "XYZ=abcdefg.log urls=abc ips~10 filtermissing==true ZZZ=",
        };

        var (filePath, urls, ips, filterMissing, includeTies) = SettingsRetriever.RetrieveInputs(args, settings);

        Assert.Contains("example.log", filePath);
        Assert.Equal(3, urls);
        Assert.Equal(3, ips);
        Assert.False(filterMissing);
        Assert.False(includeTies);
    }

    [Theory]
    [InlineData("file=abcdefg.log")]
    [InlineData("file= ")]
    public void RetrieveInputs_ShouldReturnDefaultFilePath_WhenInValidFileIsGiven(string file)
    {
        var settings = GetDefaultConfigValues();
        var args = new[] { file };

        var (filePath, _, _, _, _) = SettingsRetriever.RetrieveInputs(args, settings);

        Assert.Contains("example.log", filePath);
    }

    [Theory]
    [InlineData("urls=10", 10)]
    [InlineData("urls=0", 3)]
    [InlineData("urls=-5", 5)]
    [InlineData("urls=1.2", 1)]
    [InlineData("urls=1.6", 2)]
    public void RetrieveInputs_ShouldParseNumberOfUrls_WhenValidIntegerIsGiven(string input, int expected)
    {
        var settings = GetDefaultConfigValues();
        var args = new string[] { input };

        var (_, urls, _, _, _) = SettingsRetriever.RetrieveInputs(args, settings);

        Assert.Equal(expected, urls);
    }

    [Theory]
    [InlineData("urls=abc")]
    [InlineData("urls=null")]
    [InlineData("urls= ")]
    [InlineData("urls==")]
    [InlineData("urls=...")]
    public void RetrieveInputs_ShouldReturnDefaultUrls_WhenInvalidValuesIsGiven(string input)
    {
        var settings = GetDefaultConfigValues();
        var args = new string[] { input };

        var (_, urls, _, _, _) = SettingsRetriever.RetrieveInputs(args, settings);

        Assert.Equal(3, urls);
    }

    [Theory]
    [InlineData("ips=10", 10)]
    [InlineData("ips=0", 3)]
    [InlineData("ips=-5", 5)]
    [InlineData("ips=1.2", 1)]
    [InlineData("ips=1.6", 2)]
    [InlineData("ips=10 ", 10)]
    public void RetrieveInputs_ShouldParseNumberOfIps_WhenValidIntegerIsGiven(string input, int expected)
    {
        var settings = GetDefaultConfigValues();
        var args = new string[] { input };

        var (_, _, ips, _, _) = SettingsRetriever.RetrieveInputs(args, settings);

        Assert.Equal(expected, ips);
    }

    [Theory]
    [InlineData("ips=abc")]
    [InlineData("ips=null")]
    [InlineData("ips= ")]
    [InlineData("ips==")]
    [InlineData("ips=...")]
    public void RetrieveInputs_ShouldReturnDefaultIps_WhenInvalidValuesIsGiven(string input)
    {
        var settings = GetDefaultConfigValues();
        var args = new[] { input };

        var (_, _, ips, _, _) = SettingsRetriever.RetrieveInputs(args, settings);

        Assert.Equal(3, ips);
    }

    [Theory]
    [InlineData("filtermissing=true", "includeties=false", true, false)]
    [InlineData("filtermissing=t", "includeties=f", true, false)]
    [InlineData("filtermissing=TRUE", "includeties=FALSE", true, false)]
    [InlineData("filtermissing=0", "includeties=1", false, true)]
    [InlineData("filtermissing=no", "includeties=yes", false, true)]
    [InlineData("filtermissing=N", "includeties=y", false, true)]
    public void RetrieveInputs_ShouldParseBooleanFlags_WhenValidValueIsGiven(string missing, string ties, bool expectedMissing, bool expectedTies)
    {
        var settings = GetDefaultConfigValues();
        var args = new[] { missing, ties };

        var (_, _, _, filterMissing, includeTies) = SettingsRetriever.RetrieveInputs(args, settings);

        Assert.Equal(expectedMissing, filterMissing);
        Assert.Equal(expectedTies, includeTies);
    }

    [Theory]
    [InlineData("filtermissing=abc", "includeties=123", false, false)]
    [InlineData("filtermissing=  ", "includeties=!!", false, false)]
    public void RetrieveInputs_ShouldReturnDefaultBooleanFlags_WhenInvalidValueIsGiven(string missing, string ties, bool expectedMissing, bool expectedTies)
    {
        var settings = GetDefaultConfigValues();
        var args = new[] { missing, ties };

        var (_, _, _, filterMissing, includeTies) = SettingsRetriever.RetrieveInputs(args, settings);

        Assert.Equal(expectedMissing, filterMissing);
        Assert.Equal(expectedTies, includeTies);
    }

    private static IConfiguration GetConfig(Dictionary<string, string?> settings)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        return configuration;
    }

    private static (string FilePath, int NumberOfUrls, int NumberOfIps, bool FilterMissingField, bool IncludeTies) GetDefaultConfigValues() => ("./Data/example.log", 3, 3, false, false);

}
