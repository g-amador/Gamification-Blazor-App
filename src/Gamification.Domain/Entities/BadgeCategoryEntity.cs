namespace Gamification.Domain.Entities;

/// <summary>
/// Represents a category that groups badges together.
/// </summary>
public class BadgeCategoryEntity
{
    /// <summary>
    /// Unique identifier of the badge category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the badge category.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Application the badge category belongs to.
    /// </summary>
    public int ApplicationId { get; set; }

    /// <summary>
    /// Navigation reference to the application the badge category belongs to.
    /// </summary>
    public ApplicationEntity Application { get; set; } = null!;
}
