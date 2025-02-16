namespace Parse_URL.Configs;

public class DefaultSettings
{
    public string FilePath { get; set; } = "./Data/example.log";
    public int NumberOfUrls { get; set; } = 3;
    public int NumberOfIps { get; set; } = 3;
    public bool FilterMissingField { get; set; } = false;
    public bool IncludeTies { get; set; } = false;
}