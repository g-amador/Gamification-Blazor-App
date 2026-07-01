namespace Gamification.Domain.Entities;

/// <summary>
/// Represents a rule that awards points and/or a badge when an event occurs.
/// </summary>
public class RuleEntity
{
    /// <summary>
    /// Unique identifier of the rule.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Optional badge awarded by this rule.
    /// </summary>
    public int? BadgeId { get; set; }
    public BadgeEntity? Badge { get; set; }

    /// <summary>
    /// Number of points awarded by this rule.
    /// </summary>
    public int NumberOfPoints { get; set; }

    /// <summary>
    /// Event type that triggers this rule.
    /// </summary>
    public string OnEventType { get; set; } = string.Empty;

    /// <summary>
    /// Application the rule belongs to.
    /// </summary>
    public int ApplicationId { get; set; }
    public ApplicationEntity Application { get; set; } = null!;
}
