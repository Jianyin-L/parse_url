using Parse_URL.Utilities;

namespace Parse_URL.Configs;

public class AppSettings
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

    public AppSettings()
    {
        FilePath = DefaultFilePath;
        NumberOfUrls = DefaultNumberOfUrls;
        NumberOfIps = DefaultNumberOfIps;
        FilterMissingField = DefaultFilterMissingField;
        IncludeTies = DefaultIncludeTies;
    }

    public AppSettings Validate()
    {
        FilePath = ArgValidationHelper.ParseFilePath(FilePath, DefaultFilePath);
        NumberOfUrls = ArgValidationHelper.ParseInt(NumberOfUrls.ToString(), DefaultNumberOfUrls);
        NumberOfIps = ArgValidationHelper.ParseInt(NumberOfIps.ToString(), DefaultNumberOfIps);
        FilterMissingField = ArgValidationHelper.ParseBool(FilterMissingField.ToString(), DefaultFilterMissingField);
        IncludeTies = ArgValidationHelper.ParseBool(IncludeTies.ToString(), DefaultIncludeTies);

        return this;
    }
}