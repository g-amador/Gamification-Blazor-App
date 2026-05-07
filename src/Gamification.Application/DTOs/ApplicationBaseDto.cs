namespace Gamification.Application.DTOs;

/// <summary>
/// Base class for application creation and update operations.
/// </summary>
public class ApplicationBaseDto
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
    /// Public API key for the application.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Private API password for the application.
    /// </summary>
    public string ApiPassword { get; set; } = string.Empty;
}
