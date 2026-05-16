using Gamification.Application.DTOs.Badge;
using Gamification.Application.DTOs.Player;
using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;

namespace Gamification.Application.Services;

/// <inheritdoc />
public class PlayerService : IPlayerService
{
    private readonly IApplicationRepository _appRepo;
    private readonly IPlayerRepository _playerRepo;

    public PlayerService(IApplicationRepository appRepo, IPlayerRepository playerRepo)
    {
        _appRepo = appRepo;
        _playerRepo = playerRepo;
    }

    /// <inheritdoc />
    public async Task<List<PlayerListItemDto>> GetAllAsync(string apiKey, string apiPassword)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
            return new List<PlayerListItemDto>();

        // Load players
        var players = await _playerRepo.GetAllAsync(app.Id);

        // Map to DTOs
        return players.Select(p => new PlayerListItemDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Email = p.Email,
            NumberOfPoints = p.NumberOfPoints
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<PlayerDto?> GetByIdAsync(string apiKey, string apiPassword, int playerId)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
            return null;

        // Load player
        var player = await _playerRepo.GetByIdAsync(app.Id, playerId);
        if (player is null)
            return null;

        // Map to DTO
        return new PlayerDto
        {
            Id = player.Id,
            FirstName = player.FirstName,
            LastName = player.LastName,
            Email = player.Email,
            NumberOfPoints = player.NumberOfPoints,
            Badges = player.Badges.Select(b => new BadgeDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Icon = b.Icon
            }).ToList()
        };
    }

    /// <inheritdoc />
    public async Task<PlayerCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreatePlayerDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
            throw new InvalidOperationException("Invalid credentials.");

        // Create entity
        var entity = new PlayerEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            ApplicationId = app.Id
        };

        // Save
        await _playerRepo.AddAsync(entity);
        await _playerRepo.SaveChangesAsync();

        return new PlayerCreatedResponseDto
        {
            Status = "created",
            Url = $"/players/{entity.Id}",
            Id = entity.Id
        };
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(string apiKey, string apiPassword, int playerId, UpdatePlayerDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
            return false;

        // Load player
        var player = await _playerRepo.GetByIdAsync(app.Id, playerId);
        if (player is null)
            return false;

        // Update fields
        player.FirstName = dto.FirstName;
        player.LastName = dto.LastName;
        player.Email = dto.Email;

        // Save
        await _playerRepo.UpdateAsync(player);
        await _playerRepo.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string apiKey, string apiPassword, int playerId)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
            return false;

        // Load player
        var player = await _playerRepo.GetByIdAsync(app.Id, playerId);
        if (player is null)
            return false;

        // Delete
        await _playerRepo.DeleteAsync(player);
        await _playerRepo.SaveChangesAsync();

        return true;
    }
}
