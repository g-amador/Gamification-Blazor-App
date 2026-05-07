namespace Gamification.Application.DTOs;

/// <summary>
/// Represents an application returned by the API.
/// </summary>
public class ApplicationDto
{
    /// <summary>
    /// Unique identifier of the application.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Display name of the application.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the application.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Public API key of the application.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
}
