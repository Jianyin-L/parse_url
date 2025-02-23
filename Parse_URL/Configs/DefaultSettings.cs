using Microsoft.Extensions.Configuration;
using Parse_URL.Utilities;

namespace Parse_URL.Configs;

public class DefaultSettings
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

    public static DefaultSettings LoadFromConfig(IConfiguration config)
    {
        var settings = new DefaultSettings();

        //settings.FilePath = config.GetValue<string>($"{SectionName}:{nameof(FilePath)}", DefaultFilePath);
        //settings.NumberOfUrls = config.GetValue<int>($"{SectionName}:{nameof(NumberOfUrls)}", DefaultNumberOfUrls);
        //settings.NumberOfIps = config.GetValue<int>($"{SectionName}:{nameof(NumberOfIps)}", DefaultNumberOfIps);
        //settings.FilterMissingField = config.GetValue<bool>($"{SectionName}:{nameof(FilterMissingField)}", DefaultFilterMissingField);
        ////IncludeTies = config.GetValue<bool>($"{SectionName}:{nameof(IncludeTies)}", DefaultIncludeTies);

        settings.FilePath = ArgValidationHelper.ParseFilePath(config.GetValue<string>($"{SectionName}:{nameof(FilePath)}", DefaultFilePath), DefaultFilePath);
        settings.NumberOfUrls = ArgValidationHelper.ParseInt(config.GetValue<string>($"{SectionName}:{nameof(NumberOfUrls)}", DefaultNumberOfUrls.ToString()), DefaultNumberOfUrls);
        settings.NumberOfIps = ArgValidationHelper.ParseInt(config.GetValue<string>($"{SectionName}:{nameof(NumberOfIps)}", DefaultNumberOfIps.ToString()), DefaultNumberOfIps);
        settings.FilterMissingField = ArgValidationHelper.ParseBool(config.GetValue<string>($"{SectionName}:{nameof(FilterMissingField)}", DefaultFilterMissingField.ToString()), DefaultFilterMissingField);
        settings.IncludeTies = ArgValidationHelper.ParseBool(config.GetValue<string>($"{SectionName}:{nameof(IncludeTies)}", DefaultIncludeTies.ToString()), DefaultIncludeTies);

        return settings;

    }

    public DefaultSettings Validate()
    {
        FilePath = ArgValidationHelper.ParseFilePath(FilePath, DefaultFilePath);
        NumberOfUrls = ArgValidationHelper.ParseInt(NumberOfUrls.ToString(), DefaultNumberOfUrls);
        NumberOfIps = ArgValidationHelper.ParseInt(NumberOfIps.ToString(), DefaultNumberOfIps);
        FilterMissingField = ArgValidationHelper.ParseBool(FilterMissingField.ToString(), DefaultFilterMissingField);
        IncludeTies = ArgValidationHelper.ParseBool(IncludeTies.ToString(), DefaultIncludeTies);

        return this;
    }
}