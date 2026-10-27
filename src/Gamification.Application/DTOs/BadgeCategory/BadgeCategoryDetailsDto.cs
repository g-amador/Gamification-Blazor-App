namespace Gamification.Application.DTOs.BadgeCategory;

/// <summary>
/// Represents the detailed information of a badge category.
/// </summary>
public class BadgeCategoryDetailsDto
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
