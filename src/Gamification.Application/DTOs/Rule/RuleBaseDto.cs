namespace Gamification.Application.DTOs.Rule;

/// <summary>
/// Base class for rule creation and update.
/// </summary>
public class RuleBaseDto
{
    /// <summary>
    /// Optional badge awarded by this rule.
    /// </summary>
    public int? BadgeId { get; set; }

    /// <summary>
    /// Number of points awarded by this rule.
    /// </summary>
    public int NumberOfPoints { get; set; }

    /// <summary>
    /// Event type, by id, that triggers this rule (defined with the event types resource).
    /// </summary>
    public int EventTypeId { get; set; }
}
