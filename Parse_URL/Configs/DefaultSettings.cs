namespace Parse_URL.Configs;

public class DefaultSettings
{
    public const string SectionName = "Defaults";

    // TODO: What if the user put a file that does not exist on purpose?
    public string FilePath { get; set; } = "./Data/example.log";
    public int NumberOfUrls { get; set; } = 99;
    public int NumberOfIps { get; set; } = 99;
    public bool FilterMissingField { get; set; } = false;
    public bool IncludeTies { get; set; } = false;
}