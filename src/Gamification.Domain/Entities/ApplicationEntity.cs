namespace Gamification.Domain.Entities;

/// <summary>
/// Represents an application registered in the gamification system.
/// </summary>
public class ApplicationEntity
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
    /// Public API key used to authenticate requests.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Private API password used for secure authentication.
    /// </summary>
    public string ApiPassword { get; set; } = string.Empty;
}
