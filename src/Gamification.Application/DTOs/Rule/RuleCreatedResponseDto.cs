namespace Gamification.Application.DTOs.Rule;

/// <summary>
/// Represents the response returned after creating a rule.
/// </summary>
public class RuleCreatedResponseDto
{
    /// <summary>
    /// Status of the creation operation.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// URL of the newly created rule resource.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Unique identifier of the created rule.
    /// </summary>
    public int Id { get; set; }
}
