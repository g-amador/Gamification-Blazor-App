using Gamification.Application.DTOs.Player;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing players.
/// </summary>
public interface IPlayerService
{
    /// <summary>
    /// Retrieves all the users (players) of an application.
    /// </summary>
    Task<List<PlayerListItemDto>> GetAllAsync(string apiKey, string apiPassword);

    /// <summary>
    /// Retrieves the basic information about a specific user (player).
    /// </summary>
    Task<PlayerDto?> GetByIdAsync(string apiKey, string apiPassword, int playerId);

    /// <summary>
    /// Retrieves all badges of a specific user (player).
    /// </summary>
    Task<PlayerBadgesDto?> GetBadgesAsync(string apiKey, string apiPassword, int playerId);

    /// <summary>
    /// Adds a new user (player) to the application.
    /// </summary>
    Task<PlayerCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreatePlayerDto dto);

    /// <summary>
    /// Updates the information of a user (player).
    /// The points and badges can only be updated by events.
    /// </summary>
    Task<bool> UpdateAsync(string apiKey, string apiPassword, int playerId, UpdatePlayerDto dto);

    /// <summary>
    /// Deletes a user (player) from the application.
    /// All events linked to the player are deleted in cascade.
    /// </summary>
    Task<bool> DeleteAsync(string apiKey, string apiPassword, int playerId);
}
