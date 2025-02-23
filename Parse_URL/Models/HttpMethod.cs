namespace Parse_URL.Models;

/// <summary>
/// Represents the HTTP methods used in web requests.
/// </summary>
public enum HttpMethod
{
    /// <summary>
    /// The GET method requests a representation of the specified resource.
    /// </summary>
    GET,

    /// <summary>
    /// The POST method submits data to be processed to the specified resource.
    /// </summary>
    POST,

    /// <summary>
    /// The PUT method replaces all current representations of the target resource with the request payload.
    /// </summary>
    PUT,

    /// <summary>
    /// The DELETE method deletes the specified resource.
    /// </summary>
    DELETE,

    /// <summary>
    /// The PATCH method applies partial modifications to a resource.
    /// </summary>
    PATCH,

    /// <summary>
    /// The HEAD method requests the headers that would be returned if the HEAD request were to be sent to the specified resource.
    /// </summary>
    HEAD,

    /// <summary>
    /// The OPTIONS method returns the HTTP methods that the server supports for the specified URL.
    /// </summary>
    OPTIONS,

    /// <summary>
    /// The TRACE method performs a message loop-back test along the path to the target resource.
    /// </summary>
    TRACE,

    /// <summary>
    /// The CONNECT method establishes a tunnel to the server identified by the target resource.
    /// </summary>
    CONNECT,

    /// <summary>
    /// Represents a missing or unknown HTTP method.
    /// </summary>
    MISSING
}
