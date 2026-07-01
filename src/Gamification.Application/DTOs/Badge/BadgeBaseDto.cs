namespace Gamification.Application.DTOs.Badge;

/// <summary>
/// Base class for badge creation and update operations.
/// </summary>
public class BadgeBaseDto
{
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
