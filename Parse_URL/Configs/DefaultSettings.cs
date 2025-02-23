using Microsoft.Extensions.Configuration;
using Parse_URL.Utilities;

namespace Parse_URL.Configs;

public class DefaultSettings    // TODO: I feel this is still not clean.
{
    public const string SectionName = "Defaults";

    private const string DefaultFilePath = "./Data/example.log";
    private const int DefaultNumberOfUrls = 99;
    private const int DefaultNumberOfIps = 99;
    private const bool DefaultFilterMissingField = false;
    private const bool DefaultIncludeTies = false;

    public string FilePath { get; set; }
    public int NumberOfUrls { get; set; }
    public int NumberOfIps { get; set; }
    public bool FilterMissingField { get; set; }
    public bool IncludeTies { get; set; }

    public DefaultSettings()
    {
        FilePath = DefaultFilePath;
        NumberOfUrls = DefaultNumberOfUrls;
        NumberOfIps = DefaultNumberOfIps;
        FilterMissingField = DefaultFilterMissingField;
        IncludeTies = DefaultIncludeTies;
    }

    public static DefaultSettings LoadFromConfig(IConfiguration config) => new DefaultSettings
    {
        FilePath = SettingsProcessor.ParseFilePath(config.GetValue<string>($"{SectionName}:{nameof(FilePath)}")!) ?? DefaultFilePath,
        NumberOfUrls = SettingsProcessor.ParseInt(config.GetValue<string>($"{SectionName}:{nameof(NumberOfUrls)}")!) ?? DefaultNumberOfUrls,
        NumberOfIps = SettingsProcessor.ParseInt(config.GetValue<string>($"{SectionName}:{nameof(NumberOfIps)}")!) ?? DefaultNumberOfIps,
        FilterMissingField = SettingsProcessor.ParseBool(config.GetValue<string>($"{SectionName}:{nameof(FilterMissingField)}")!) ?? DefaultFilterMissingField,
        IncludeTies = SettingsProcessor.ParseBool(config.GetValue<string>($"{SectionName}:{nameof(IncludeTies)}")!) ?? DefaultIncludeTies
    };
}