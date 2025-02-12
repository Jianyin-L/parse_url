namespace Parse_URL.Model;

public class LogEntry
{
    public string IPAddress { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public string HttpMethod { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public int ResponseSize { get; set; }
    public string UserAgent { get; set; } = string.Empty;
}