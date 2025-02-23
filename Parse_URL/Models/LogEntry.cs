namespace Parse_URL.Models;

/// <summary>
/// Represents a log entry containing information about a request.
/// </summary>
public class LogEntry
{
    /// <summary>
    /// Gets or sets the IP address of the client making the request.
    /// </summary>
    public string IPAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user associated with the request.
    /// </summary>
    public string User { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp of the request.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the HTTP method of the request.
    /// </summary>
    public HttpMethod Method { get; set; }

    /// <summary>
    /// Gets or sets the URL of the request.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status code of the response.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the size of the response in bytes.
    /// </summary>
    public int ResponseSize { get; set; }

    /// <summary>
    /// Gets or sets the user agent of the client making the request.
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;
}