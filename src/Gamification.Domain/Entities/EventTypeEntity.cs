namespace Gamification.Domain.Entities;

/// <summary>
/// Represents a type of event defined by an application.
/// All events that are recorded by the gamification API are associated with a type.
/// The types can be defined by the application using the API with the related API calls,
/// then event types can be used in rules to define the conditions needed to obtain badges.
/// </summary>
public class EventTypeEntity
{
    /// <summary>
    /// Unique identifier of the event type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the event type.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Application the event type belongs to.
    /// </summary>
    public int ApplicationId { get; set; }
    public ApplicationEntity Application { get; set; } = null!;
}
