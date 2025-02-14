using System.Globalization;

namespace Parse_URL.Utilities;

public static class ArgumentsParser
{
    // TODO: Move to a configuration file? Will it works in exe? Or at least a static class?
    private const string DefaultData = "./Data/example.log";
    private const int DefaultUrls = 3;
    private const int DefaultIps = 3;
    private const bool DefaultFilterMissingField = false;
    private const bool DefaultIncludeTies = false;

    public static (string filePath, int numberOfUrls, int numberOfIps, bool filterMissingField, bool includeTies) ParseArguments(string[] args)
    {
        // Simplify these?
        var path = Path.Combine(Directory.GetCurrentDirectory(), DefaultData);
        var numberOfUrls = DefaultUrls;
        var numberOfIps = DefaultIps;
        var filterMissingField = DefaultFilterMissingField;
        var includeTies = DefaultIncludeTies;

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
                        Console.WriteLine($"File does not exist. Use Default: {DefaultData}\n");
                    }
                    break;
                case "urls":
                    numberOfUrls = ParsePositiveInt(value, DefaultUrls);
                    break;
                case "ips":
                    numberOfIps = ParsePositiveInt(value, DefaultIps);
                    break;
                case "filtermissing":
                    filterMissingField = ParseBool(value, DefaultFilterMissingField);
                    break;
                case "includeties":
                    includeTies = ParseBool(value, DefaultIncludeTies);
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
