namespace Parse_URL.Model;

public class LogEntry
{
    public string IPAddress { get; set; }
    public DateTime Timestamp { get; set; }
    public string HttpMethod { get; set; }
    public string Url { get; set; }
    public int StatusCode { get; set; }
    public int ResponseSize { get; set; }
    public string UserAgent { get; set; }
}