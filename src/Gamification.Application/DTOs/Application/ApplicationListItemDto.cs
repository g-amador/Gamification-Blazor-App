namespace Gamification.Application.DTOs.Application;

/// <summary>
/// Represents an application in list responses.
/// Only public information is exposed; apiKey and apiPassword are never returned in lists.
/// </summary>
public class ApplicationListItemDto
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
}
