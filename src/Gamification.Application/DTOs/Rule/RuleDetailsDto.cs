namespace Gamification.Application.DTOs.Rule;

/// <summary>
/// Represents detailed information about a rule.
/// </summary>
public class RuleDetailsDto
{
    /// <summary>
    /// Unique identifier of the rule.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Optional badge awarded by this rule.
    /// </summary>
    public int? BadgeId { get; set; }

    /// <summary>
    /// Number of points awarded by this rule.
    /// </summary>
    public int NumberOfPoints { get; set; }

    /// <summary>
    /// Identifier of the event type that triggers this rule.
    /// </summary>
    public int EventTypeId { get; set; }

    /// <summary>
    /// Name of the event type that triggers this rule (read convenience only).
    /// </summary>
    public string EventTypeName { get; set; } = string.Empty;
}
