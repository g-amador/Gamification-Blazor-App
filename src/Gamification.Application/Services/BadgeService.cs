using Gamification.Application.DTOs.Badge;
using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;

namespace Gamification.Application.Services;

/// <inheritdoc />
public class BadgeService : IBadgeService
{
    private readonly IApplicationRepository _appRepo;
    private readonly IBadgeRepository _badgeRepo;

    public BadgeService(IApplicationRepository appRepo, IBadgeRepository badgeRepo)
    {
        _appRepo = appRepo;
        _badgeRepo = badgeRepo;
    }

    /// <inheritdoc />
    public async Task<List<BadgeListItemDto>> GetAllAsync(string apiKey, string apiPassword)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            return [];
        }

        // Load badges
        var badges = await _badgeRepo.GetAllAsync(app.Id);

        // Map to DTOs
        return badges.Select(b => new BadgeListItemDto
        {
            Id = b.Id,
            Name = b.Name,
            Description = b.Description,
            Icon = b.Icon
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<BadgeDto?> GetByIdAsync(string apiKey, string apiPassword, int badgeId)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            return null;
        }

        // Load badge
        var badge = await _badgeRepo.GetByIdAsync(app.Id, badgeId);
        if (badge is null)
        {
            return null;
        }

        // Map to DTO
        return new BadgeDto
        {
            Id = badge.Id,
            Name = badge.Name,
            Description = badge.Description,
            Icon = badge.Icon
        };
    }

    /// <inheritdoc />
    public async Task<BadgeCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateBadgeDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        // Create entity
        var entity = new BadgeEntity
        {
            Name = dto.Name,
            Description = dto.Description,
            Icon = dto.Icon,
            ApplicationId = app.Id
        };

        // Save
        await _badgeRepo.AddAsync(entity);
        await _badgeRepo.SaveChangesAsync();

        return new BadgeCreatedResponseDto
        {
            Status = "created",
            Url = $"/badges/{entity.Id}",
            Id = entity.Id
        };
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(string apiKey, string apiPassword, int badgeId, UpdateBadgeDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            return false;
        }

        // Load badge
        var badge = await _badgeRepo.GetByIdAsync(app.Id, badgeId);
        if (badge is null)
        {
            return false;
        }

        // Update fields
        badge.Name = dto.Name;
        badge.Description = dto.Description;
        badge.Icon = dto.Icon;

        // Save
        await _badgeRepo.UpdateAsync(badge);
        await _badgeRepo.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string apiKey, string apiPassword, int badgeId)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            return false;
        }

        // Load badge
        var badge = await _badgeRepo.GetByIdAsync(app.Id, badgeId);
        if (badge is null)
        {
            return false;
        }

        // Delete
        await _badgeRepo.DeleteAsync(badge);
        await _badgeRepo.SaveChangesAsync();

        return true;
    }
}
