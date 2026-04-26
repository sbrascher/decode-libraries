namespace Decode.AspNetCore.Models;

/// <summary>
/// Standard structure for all API responses in the Decode ecosystem.
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Indicates if the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The payload returned by the operation.
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// A collection of error messages, if any.
    /// </summary>
    public IEnumerable<string>? Errors { get; set; }

    /// <summary>
    /// A unique identifier for the error, useful for log correlation.
    /// </summary>
    public string? ErrorId { get; set; }

    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public ApiResponse() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse"/> class.
    /// </summary>
    public ApiResponse(bool success, object? data = null, IEnumerable<string>? errors = null, string? errorId = null)
    {
        Success = success;
        Data = data;
        Errors = errors;
        ErrorId = errorId;
    }
}