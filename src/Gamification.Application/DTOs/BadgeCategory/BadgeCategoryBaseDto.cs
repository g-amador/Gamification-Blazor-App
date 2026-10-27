namespace Gamification.Application.DTOs.BadgeCategory;

/// <summary>
/// Base class for badge category creation and update operations.
/// </summary>
public class BadgeCategoryBaseDto
{
    /// <summary>
    /// Name of the badge category.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
