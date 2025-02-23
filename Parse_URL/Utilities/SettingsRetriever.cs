using Microsoft.Extensions.Configuration;
using Parse_URL.Configs;

namespace Parse_URL.Utilities;

public static class SettingsRetriever
{
    public static (string FilePath, int NumberOfUrls, int NumberOfIps, bool FilterMissingField, bool IncludeTies) RetrieveFromConfig(IConfiguration config) => (
        SettingsProcessor.ParseFilePath(config.GetValue<string>($"{DefaultSettings.SectionName}:{nameof(DefaultSettings.FilePath)}", DefaultSettings.FilePath)) ?? DefaultSettings.FilePath,
        SettingsProcessor.ParseInt(config.GetValue<string>($"{DefaultSettings.SectionName}:{nameof(DefaultSettings.NumberOfUrls)}", DefaultSettings.NumberOfUrls.ToString())) ?? DefaultSettings.NumberOfUrls,
        SettingsProcessor.ParseInt(config.GetValue<string>($"{DefaultSettings.SectionName}:{nameof(DefaultSettings.NumberOfIps)}", DefaultSettings.NumberOfIps.ToString())) ?? DefaultSettings.NumberOfIps,
        SettingsProcessor.ParseBool(config.GetValue<string>($"{DefaultSettings.SectionName}:{nameof(DefaultSettings.FilterMissingField)}", DefaultSettings.FilterMissingField.ToString())) ?? DefaultSettings.FilterMissingField,
        SettingsProcessor.ParseBool(config.GetValue<string>($"{DefaultSettings.SectionName}:{nameof(DefaultSettings.IncludeTies)}", DefaultSettings.IncludeTies.ToString())) ?? DefaultSettings.IncludeTies
    );

    public static (string FilePath, int NumberOfUrls, int NumberOfIps, bool FilterMissingField, bool IncludeTies) RetrieveArguments(string[] args, (string FilePath, int NumberOfUrls, int NumberOfIps, bool FilterMissingField, bool IncludeTies) defaults)
    {
        string? path = null;
        int? numberOfUrls = null;
        int? numberOfIps = null;
        bool? filterMissingField = null;
        bool? includeTies = null;

        foreach (var arg in args)
        {
            var parts = arg.Split('=', 2, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) continue;

            var key = parts[0].ToLower();
            var value = parts[1].Trim();

            switch (key)
            {
                case "file":
                    path = SettingsProcessor.ParseFilePath(value);
                    break;
                case "urls":
                    numberOfUrls = SettingsProcessor.ParseInt(value);
                    break;
                case "ips":
                    numberOfIps = SettingsProcessor.ParseInt(value);
                    break;
                case "filtermissing":
                    filterMissingField = SettingsProcessor.ParseBool(value);
                    break;
                case "includeties":
                    includeTies = SettingsProcessor.ParseBool(value);
                    break;
                default:
                    Console.WriteLine($"Unknown argument '{key}' will be ignored.");
                    break;
            }
        }

        return (path ?? defaults.FilePath, 
            numberOfUrls ?? defaults.NumberOfUrls, 
            numberOfIps ?? defaults.NumberOfIps,
            filterMissingField ?? defaults.FilterMissingField, 
            includeTies ?? defaults.IncludeTies);
    }

}
