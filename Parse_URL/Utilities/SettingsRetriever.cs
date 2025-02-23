using Parse_URL.Configs;

namespace Parse_URL.Utilities;

public static class SettingsRetriever
{
    public static (string FilePath, int NumberOfUrls, int NumberOfIps, bool FilterMissingField, bool IncludeTies) RetrieveArguments(string[] args, DefaultSettings defaults)
    {
        // TODO: Possible to simpifly this? The defaults.DefaultXXX is being used twice
        var path = Path.Combine(Directory.GetCurrentDirectory(), defaults.FilePath);
        var numberOfUrls = defaults.NumberOfUrls;
        var numberOfIps = defaults.NumberOfIps;
        var filterMissingField = defaults.FilterMissingField;
        var includeTies = defaults.IncludeTies;

        foreach (var arg in args)
        {
            var parts = arg.Split('=', 2, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) continue;

            var key = parts[0].ToLower();
            var value = parts[1].Trim();

            switch (key)
            {
                case "file":
                    path = SettingsProcessor.ParseFilePath(value, defaults.FilePath);
                    break;
                case "urls":
                    numberOfUrls = SettingsProcessor.ParseInt(value, defaults.NumberOfUrls);
                    break;
                case "ips":
                    numberOfIps = SettingsProcessor.ParseInt(value, defaults.NumberOfIps);
                    break;
                case "filtermissing":
                    filterMissingField = SettingsProcessor.ParseBool(value, defaults.FilterMissingField);
                    break;
                case "includeties":
                    includeTies = SettingsProcessor.ParseBool(value, defaults.IncludeTies);
                    break;
                default:
                    Console.WriteLine($"Unknown argument '{key}' will be ignored.");
                    break;
            }
        }

        return (path, numberOfUrls, numberOfIps, filterMissingField, includeTies);
    }

}
