namespace Parse_URL.Models;

/// <summary>
/// Assuming these are the only valid HTTP methods
/// </summary>
public enum HttpMethod
{
    GET,
    POST,
    PUT,
    DELETE,
    PATCH,
    HEAD,
    OPTIONS,
    TRACE,
    CONNECT,
    MISSING
}
