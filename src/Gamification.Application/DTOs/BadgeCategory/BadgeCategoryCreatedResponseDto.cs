namespace Gamification.Application.DTOs.BadgeCategory;

/// <summary>
/// Represents the response returned when a badge category is created.
/// </summary>
public class BadgeCategoryCreatedResponseDto
{
    /// <summary>
    /// Status of the creation operation.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Relative URL of the newly created badge category.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Unique identifier of the newly created badge category.
    /// </summary>
    public int Id { get; set; }
}
