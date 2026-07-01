namespace Gamification.Domain.Entities;

/// <summary>
/// Represents a badge that belongs to an application.
/// </summary>
public class BadgeEntity
{
    /// <summary>
    /// Unique identifier of the badge.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the badge.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the badge.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Path or URL to the badge icon.
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Application the badge belongs to.
    /// </summary>
    public int ApplicationId { get; set; }
    public ApplicationEntity Application { get; set; } = null!;
}
