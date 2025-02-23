namespace Parse_URL.Configs;

/// <summary>
/// Provides default settings for the URL parsing functionality.
/// </summary>
public static class DefaultSettings
{
    /// <summary>
    /// The name of the section in the configuration file that contains the default settings.
    /// </summary>
    public const string SectionName = "Defaults";

    /// <summary>
    /// The default file path of the log file used for parsing URLs.
    /// </summary>
    public const string FilePath = "./Data/example.log";

    /// <summary>
    /// The default number of URLs to return.
    /// </summary>
    public const int NumberOfUrls = 99;

    /// <summary>
    /// The default number of IPs to return.
    /// </summary>
    public const int NumberOfIps = 99;

    /// <summary>
    /// Specifies whether to filter out URLs with missing fields during parsing.
    /// </summary>
    public const bool FilterMissingField = false;

    /// <summary>
    /// Specifies whether to include ties when ranking URLs.
    /// </summary>
    public const bool IncludeTies = false;
}
