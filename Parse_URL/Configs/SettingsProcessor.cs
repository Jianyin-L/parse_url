using System.Globalization;

namespace Parse_URL.Configs;

/// <summary>
/// A static class that provides methods for parsing different types of settings.
/// </summary>
public static class SettingsProcessor
{
    /// <summary>
    /// Parses a file path value.
    /// </summary>
    /// <param name="value">The file path value to parse.</param>
    /// <returns>The parsed file path if it is valid and exists; otherwise, null.</returns>
    public static string? ParseFilePath(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !File.Exists(value))
        {
            Console.WriteLine($"'{value}' is not a valid file path or the file does not exist. Use default value instead.");
            return null;
        }

        return value;
    }

    /// <summary>
    /// Parses an integer value.
    /// </summary>
    /// <param name="value">The integer value to parse.</param>
    /// <returns>The parsed integer if it is valid; otherwise, null.</returns>
    public static int? ParseInt(string value)
    {
        if (!string.Equals(value, "0") && double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
        {
            return (int)Math.Round(Math.Abs(number));
        }

        Console.WriteLine($"'{value}' is not a valid value. Use default value instead.");
        return null;
    }

    /// <summary>
    /// Parses a boolean value.
    /// </summary>
    /// <param name="value">The boolean value to parse.</param>
    /// <returns>The parsed boolean if it is valid; otherwise, null.</returns>
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
                Console.WriteLine($"'{value}' is not a valid value. Use default value instead.");
                return null;
        }
    }
}
