using System.Globalization;

namespace Parse_URL.Utilities;

public static class SettingsProcessor
{
    public static string? ParseFilePath(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !File.Exists(value))
        {
            Console.WriteLine($"'{value}' is not a valid value or it does not exist.");
            return null;
        }

        return value;
    }

    public static int? ParseInt(string value)
    {
        if (!string.Equals(value, "0") && (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number)))
        {
            return (int)Math.Round(Math.Abs(number));
        }

        Console.WriteLine($"'{value}' is not a valid value.");
        return null;
    }

    public static bool? ParseBool(string value)
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
                Console.WriteLine($"'{value}' is not a valid value.");
                return null;
        }
    }
}
