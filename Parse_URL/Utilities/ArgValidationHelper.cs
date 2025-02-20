﻿using System.Globalization;

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
