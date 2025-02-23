using Microsoft.Extensions.Configuration;
using Parse_URL.Utilities;

namespace Parse_URL.Tests;

public class DefaultSettingsTests
{
    private static IConfiguration GetConfig(Dictionary<string, string?> settings)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        return configuration;
    }

    [Fact]
    public void DefaultSettings_ShouldReturnConfig_WhenValidConfigIsProvided()
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
        
        var settings = SettingsRetriever.RetrieveFromConfig(config);

        Assert.Contains("example.log", settings.FilePath);
        Assert.Equal(9, settings.NumberOfUrls);
        Assert.Equal(9, settings.NumberOfIps);
        Assert.True(settings.FilterMissingField);
        Assert.False(settings.IncludeTies);
    }

    [Fact]
    public void DefaultSettings_ShouldFallbackToDefaults_WhenInvalidConfigIsProvided()
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

        var settings = SettingsRetriever.RetrieveFromConfig(config);

        Assert.Contains("example.log", settings.FilePath);
        Assert.Equal(99, settings.NumberOfUrls);    // TODO: Hardcode the value here may not be good. Use default.NumberOfUrls? 
        Assert.Equal(99, settings.NumberOfIps);
        Assert.False(settings.FilterMissingField);
        Assert.False(settings.IncludeTies);
    }

    [Fact]
    public void DefaultSettings_ShouldReturnDefaults_WhenInvalidConfigArgumentIsProvided()
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

        var settings = SettingsRetriever.RetrieveFromConfig(config);

        Assert.Contains("example.log", settings.FilePath);
        Assert.Equal(99, settings.NumberOfUrls);
        Assert.Equal(99, settings.NumberOfIps);
        Assert.False(settings.FilterMissingField);
        Assert.False(settings.IncludeTies);
    }
}
