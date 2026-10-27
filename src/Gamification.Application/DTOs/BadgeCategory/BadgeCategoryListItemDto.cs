namespace Gamification.Application.DTOs.BadgeCategory;

/// <summary>
/// Represents a badge category in list responses.
/// </summary>
public class BadgeCategoryListItemDto
{
    /// <summary>
    /// Unique identifier of the badge category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the badge category.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
