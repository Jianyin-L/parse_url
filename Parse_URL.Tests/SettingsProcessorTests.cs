using Parse_URL.Configs;

namespace Parse_URL.Tests;

public class SettingsProcessorTests
{
    [Theory]
    [InlineData("./Data/example.log", true)]
    [InlineData("invalid/path", false)]
    [InlineData("", false)]
    [InlineData("null", false)]
    public void ParseFilePath_ShouldReturnExpectedResult(string value, bool expectedValid)
    {
        var result = SettingsProcessor.ParseFilePath(value);
        if (expectedValid)
        {
            Assert.Equal(value, result);
        }
        else
        {
            Assert.Null(result);
        }
    }

    [Theory]
    [InlineData("42", 42)]
    [InlineData("-42", 42)]
    [InlineData("0", null)]
    [InlineData("invalid", null)]
    [InlineData(" ", null)]
    [InlineData("null", null)]
    public void ParseInt_ShouldReturnExpectedResult(string value, int? expected)
    {
        var result = SettingsProcessor.ParseInt(value);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("tRUe", true)]
    [InlineData("t", true)]
    [InlineData("yes", true)]
    [InlineData("y", true)]
    [InlineData("1", true)]
    [InlineData("false", false)]
    [InlineData("f", false)]
    [InlineData("no", false)]
    [InlineData("n", false)]
    [InlineData("0", false)]
    [InlineData("invalid", null)]
    [InlineData("null", null)]
    [InlineData("999", null)]
    [InlineData(" ", null)]
    [InlineData("", null)]
    [InlineData("...", null)]
    public void ParseBool_ShouldReturnExpectedResult(string value, bool? expected)
    {
        var result = SettingsProcessor.ParseBool(value);
        Assert.Equal(expected, result);
    }
}
