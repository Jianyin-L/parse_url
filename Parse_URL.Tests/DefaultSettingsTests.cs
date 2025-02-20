using Parse_URL.Configs;
using Microsoft.Extensions.Configuration;

namespace Parse_URL.Tests;

public class DefaultSettingsTests
{
    private static IConfiguration GetMockConfig(Dictionary<string, string?> settings)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        return configuration;
    }

    [Fact]
    public void DefaultSettings_ShouldLoadFromConfig_WhenConfigIsProvided()
    {
        var settings = new Dictionary<string, string?>
        {
            { "Defaults:FilePath", "XYZ.log" },
            { "Defaults:NumberOfUrls", "9" },
            { "Defaults:NumberOfIps", "9" },
            { "Defaults:FilterMissingField", "true" },
            { "Defaults:IncludeTies", "false" }
        };
        var config = GetMockConfig(settings);
        var defaultSettings = new DefaultSettings();

        config.GetSection("Defaults").Bind(defaultSettings);

        Assert.Equal("XYZ.log", defaultSettings.FilePath);
        Assert.Equal(9, defaultSettings.NumberOfUrls);
        Assert.Equal(9, defaultSettings.NumberOfIps);
        Assert.True(defaultSettings.FilterMissingField);
        Assert.False(defaultSettings.IncludeTies);
    }

    [Fact]
    public void DefaultSettings_ShouldFallbackToDefaults_WhenConfigIsMissing()
    {
        var settings = new Dictionary<string, string?>
        {
            { "Defaults:FilePath", null },
            { "Defaults:NumberOfUrls", "" },
            { "Defaults:NumberOfIps", "     " },
            { "FilterMissingField", "true" },
            { "XYZ:IncludeTies", "false" }
        };
        var config = GetMockConfig(settings);
        var defaultSettings = new DefaultSettings();

        config.GetSection("Defaults").Bind(defaultSettings);

        Assert.Equal("./Data/example.log", defaultSettings.FilePath);
        Assert.Equal(99, defaultSettings.NumberOfUrls);
        Assert.Equal(99, defaultSettings.NumberOfIps);
        Assert.False(defaultSettings.FilterMissingField);
        Assert.False(defaultSettings.IncludeTies);
    }
}
