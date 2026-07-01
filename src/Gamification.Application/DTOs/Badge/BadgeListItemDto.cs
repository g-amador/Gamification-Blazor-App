namespace Gamification.Application.DTOs.Badge;

/// <summary>
/// Represents a badge in list responses.
/// </summary>
public class BadgeListItemDto
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
}
