namespace Gamification.Application.DTOs.Application;

/// <summary>
/// Represents an application returned by the API.
/// </summary>
public class ApplicationDto
{
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
