namespace Gamification.Application.DTOs.Badge;

/// <summary>
/// Represents the response returned after creating a badge.
/// </summary>
public class BadgeCreatedResponseDto
{
    /// <summary>
    /// Status of the creation operation.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// URL of the newly created badge resource.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Unique identifier of the created badge.
    /// </summary>
    public int Id { get; set; }
}
