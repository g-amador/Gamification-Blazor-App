namespace Gamification.Application.DTOs.Player;

/// <summary>
/// Base class for player creation and update operations.
/// </summary>
public class PlayerBaseDto
{
    /// <summary>
    /// First name of the player.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name of the player.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Email of the player.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
