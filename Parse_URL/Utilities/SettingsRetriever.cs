using Parse_URL.Configs;

namespace Parse_URL.Utilities;

public static class SettingsRetriever
{
    public static (string FilePath, int NumberOfUrls, int NumberOfIps, bool FilterMissingField, bool IncludeTies) RetrieveArguments(string[] args, DefaultSettings defaults)
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
