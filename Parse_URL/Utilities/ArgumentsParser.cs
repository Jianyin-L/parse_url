using System.Globalization;

namespace Parse_URL.Utilities;

public static class ArgumentsParser
{
    private const string DefaultData = "./Data/example.log";

    public static (string filePath, int numberOfUrls, int numberOfIps, bool filterMissingField, bool includeTies) ParseArguments(string[] args)
    {
        // TODO: Move to a configuration file? Will it works in exe? 
        // Default values
        var path = Path.Combine(Directory.GetCurrentDirectory(), DefaultData);
        var numberOfUrls = 3;
        var numberOfIps = 3;
        var filterMissingField = false;
        var includeTies = false;

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
                    numberOfUrls = ParsePositiveInt(value, numberOfUrls);
                    break;
                case "ips":
                    numberOfIps = ParsePositiveInt(value, numberOfIps);
                    break;
                case "filtermissing":
                    filterMissingField = ParseBool(value);
                    break;
                case "includeties":
                    includeTies = ParseBool(value);
                    break;
            }
        }

        return (path, numberOfUrls, numberOfIps, filterMissingField, includeTies);
    }

    private static int ParsePositiveInt(string value, int defaultValue)
    {
        if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
        {
            return (int)Math.Round(Math.Abs(number)); // Round and ensure it's at least 1
        }
        return defaultValue;
    }

    private static bool ParseBool(string value)
    {
        return value.ToLower() switch
        {
            "true" or "yes" or "1" or "y" or "t" => true,
            "false" or "no" or "0" or "n" or "f" => false,
            _ => false // Default to false if invalid input
        };
    }
}
