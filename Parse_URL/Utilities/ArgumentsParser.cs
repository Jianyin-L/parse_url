using Parse_URL.Configs;
using System.Globalization;

namespace Parse_URL.Utilities;

public static class ArgumentsParser
{
    public static (string FilePath, int NumberOfUrls, int NumberOfIps, bool FilterMissingField, bool IncludeTies) ParseArguments(string[] args, DefaultSettings defaults)
    {
        // Possible to simpifly this? The defaults.DefaultXXX is being used twice
        var path = Path.Combine(Directory.GetCurrentDirectory(), defaults.DefaultData);
        var numberOfUrls = defaults.DefaultUrls;
        var numberOfIps = defaults.DefaultIps;
        var filterMissingField = defaults.DefaultFilterMissingField;
        var includeTies = defaults.DefaultIncludeTies;

        foreach (var arg in args)
        {
            var parts = arg.Split('=', 2, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) continue;

            var key = parts[0].ToLower();
            var value = parts[1].Trim();

            switch (key)
            {
                case "file":
                    if (File.Exists(value))
                    {
                        path = value;
                    }
                    else
                    {
                        Console.WriteLine($"File does not exist. Use Default: {defaults.DefaultData}\n");
                    }
                    break;
                case "urls":
                    numberOfUrls = ParsePositiveInt(value, defaults.DefaultUrls);
                    break;
                case "ips":
                    numberOfIps = ParsePositiveInt(value, defaults.DefaultIps);
                    break;
                case "filtermissing":
                    filterMissingField = ParseBool(value, defaults.DefaultFilterMissingField);
                    break;
                case "includeties":
                    includeTies = ParseBool(value, defaults.DefaultIncludeTies);
                    break;
            }
        }

        return (path, numberOfUrls, numberOfIps, filterMissingField, includeTies);
    }

    private static int ParsePositiveInt(string value, int defaultValue)
    {
        if (!string.Equals(value, "0") && (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number)))
        {
            return (int)Math.Round(Math.Abs(number));
        }
        Console.WriteLine($"'{value}' is not a valid value. Defaulting to {defaultValue}.");
        return defaultValue;
    }

    private static bool ParseBool(string value, bool defaultValue)
    {
        value = value.Trim().ToLower();

        switch (value)
        {
            case "true":
            case "t":
            case "yes":
            case "y":
            case "1":
                return true;

            case "false":
            case "f":
            case "no":
            case "n":
            case "0":
                return false;

            default:
                Console.WriteLine($"'{value}' is not a valid value. Defaulting to {defaultValue}.");
                return false;
        }
    }
}
