namespace Gamification.Application.DTOs.Player;

/// <summary>
/// Represents the response returned after creating a player.
/// </summary>
public class PlayerCreatedResponseDto
{
    /// <summary>
    /// Status of the creation operation.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// URL of the newly created player resource.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Unique identifier of the created player.
    /// </summary>
    public int Id { get; set; }
}
