using Gamification.Application.DTOs.Player;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing players.
/// </summary>
public interface IPlayerService
{
    /// <summary>
    /// Retrieves all players of an application.
    /// </summary>
    Task<List<PlayerListItemDto>> GetAllAsync(string apiKey, string apiPassword);

    /// <summary>
    /// Retrieves detailed information about a player.
    /// </summary>
    Task<PlayerDto?> GetByIdAsync(string apiKey, string apiPassword, int playerId);

    /// <summary>
    /// Creates a new player.
    /// </summary>
    Task<PlayerCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreatePlayerDto dto);

    /// <summary>
    /// Updates a player's basic information.
    /// </summary>
    Task<bool> UpdateAsync(string apiKey, string apiPassword, int playerId, UpdatePlayerDto dto);

    /// <summary>
    /// Deletes a player.
    /// </summary>
    Task<bool> DeleteAsync(string apiKey, string apiPassword, int playerId);
}
