using Parse_URL.Configs;
using Microsoft.Extensions.Configuration;

namespace Parse_URL.Tests;

public class AppSettingsTests
{
    private static IConfiguration GetMockConfig(Dictionary<string, string?> settings)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        return configuration;
    }

    [Fact]
    public void DefaultSettings_ShouldLoadFromConfig_WhenValidConfigIsProvided()
    {
        var settings = new Dictionary<string, string?>
        {
            { "Defaults:NumberOfUrls", "9" },
            { "Defaults:NumberOfIps", "9" },
            { "Defaults:FilterMissingField", "true" },
            { "Defaults:IncludeTies", "false" }
        };
        var config = GetMockConfig(settings);
        var defaultSettings = new AppSettings();

        config.GetSection("Defaults").Bind(defaultSettings);
        defaultSettings.Validate();

        Assert.Equal("./Data/example.log", defaultSettings.FilePath);
        Assert.Equal(9, defaultSettings.NumberOfUrls);
        Assert.Equal(9, defaultSettings.NumberOfIps);
        Assert.True(defaultSettings.FilterMissingField);
        Assert.False(defaultSettings.IncludeTies);
    }

    [Fact]
    public void DefaultSettings_ShouldFallbackToDefaults_WhenInvalidConfigIsProvided()
    {
        var settings = new Dictionary<string, string?>
        {
            { "Defaults:FilePath", null },
            { "Defaults:NumberOfUrls", "-1" },
            { "Defaults:NumberOfIps", "-9" },
            { "FilterMissingField", "true" },
            { "XYZ:IncludeTies", "false" }
        };
        var config = GetMockConfig(settings);

        var defaultSettings = new AppSettings();
        config.GetSection("Defaults").Bind(defaultSettings);
        defaultSettings.Validate();

        Assert.Equal("./Data/example.log", defaultSettings.FilePath);
        Assert.Equal(99, defaultSettings.NumberOfUrls);
        Assert.Equal(99, defaultSettings.NumberOfIps);
        Assert.False(defaultSettings.FilterMissingField);
        Assert.False(defaultSettings.IncludeTies);
    }
}
