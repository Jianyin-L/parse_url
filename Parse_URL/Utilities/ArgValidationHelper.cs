using System.Globalization;

namespace Parse_URL.Utilities;

public static class ArgValidationHelper
{
    public static string ParseFilePath(string value, string defaultValue)
    {
        if (string.IsNullOrWhiteSpace(value) || !File.Exists(value))
        {
            Console.WriteLine($"'{value}' is not a valid value or it does not exist. Defaulting to {defaultValue}.");
            return defaultValue;
        }
        return value;
    }

    public static int ParseInt(string value, int defaultValue)
    {
        if (!string.Equals(value, "0") && (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number)))
        {
            return (int)Math.Round(Math.Abs(number));
        }
        Console.WriteLine($"'{value}' is not a valid value. Defaulting to {defaultValue}.");
        return defaultValue;
    }

    public static bool ParseBool(string value, bool defaultValue)
    {
        if (IsValidBool(value))
        {
            value = value.Trim().ToLower();
            return value switch
            {
                "true" or "t" or "yes" or "y" or "1" => true,
                "false" or "f" or "no" or "n" or "0" => false,
                _ => defaultValue
            };
        }
        else
        {
            Console.WriteLine($"'{value}' is not a valid value. Defaulting to {defaultValue}.");
            return defaultValue;
        }
    }

    private static bool IsValidBool(string value) => value.Trim().ToLower() switch
    {
        "true" or "t" or "yes" or "y" or "1" or "false" or "f" or "no" or "n" or "0" => true,
        _ => false,
    };
}
