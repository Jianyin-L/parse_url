namespace Parse_URL.Configs;

public class DefaultSettings
{
    public string DefaultData { get; set; } = "./Data/example.log";
    public int DefaultUrls { get; set; } = 3;
    public int DefaultIps { get; set; } = 3;
    public bool DefaultFilterMissingField { get; set; } = false;
    public bool DefaultIncludeTies { get; set; } = false;
}