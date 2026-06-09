using Gamification.Application.DTOs.Event;
using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;

namespace Gamification.Application.Services;

/// <inheritdoc />
public class EventService : IEventService
{
    private readonly IApplicationRepository _appRepo;
    private readonly IEventRepository _eventRepo;
    private readonly IPlayerRepository _playerRepo;
    private readonly IRuleRepository _ruleRepo;

    public EventService(
        IApplicationRepository appRepo,
        IEventRepository eventRepo,
        IPlayerRepository playerRepo,
        IRuleRepository ruleRepo)
    {
        _appRepo = appRepo;
        _eventRepo = eventRepo;
        _playerRepo = playerRepo;
        _ruleRepo = ruleRepo;
    }

    /// <inheritdoc />
    public async Task<List<EventListItemDto>> GetAllAsync(string apiKey, string apiPassword)
    {
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            return [];
        }

        var events = await _eventRepo.GetAllAsync(app.Id);

        return events.Select(e => new EventListItemDto
        {
            Id = e.Id,
            PlayerId = e.PlayerId,
            Type = e.Type,
            Timestamp = e.Timestamp
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<EventDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int eventId)
    {
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            return null;
        }

        var ev = await _eventRepo.GetByIdAsync(app.Id, eventId);
        if (ev is null)
        {
            return null;
        }

        return new EventDetailsDto
        {
            Id = ev.Id,
            PlayerId = ev.PlayerId,
            Type = ev.Type,
            Timestamp = ev.Timestamp
        };
    }

    /// <inheritdoc />
    public async Task<EventCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateEventDto dto)
    {
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        // Load player
        var player = await _playerRepo.GetByIdAsync(app.Id, dto.PlayerId);
        if (player is null)
        {
            throw new InvalidOperationException("Player not found.");
        }

        // Create event
        var ev = new EventEntity
        {
            PlayerId = dto.PlayerId,
            Type = dto.Type,
            Timestamp = dto.Timestamp,
            ApplicationId = app.Id
        };

        await _eventRepo.AddAsync(ev);

        // Apply rules
        var rules = await _ruleRepo.GetAllAsync(app.Id);

        foreach (var rule in rules.Where(r => r.OnEventType == dto.Type))
        {
            // Add points
            player.NumberOfPoints += rule.NumberOfPoints;

            // Add badge if not already earned
            if (rule.BadgeId.HasValue &&
                !player.Badges.Any(b => b.Id == rule.BadgeId.Value))
            {
                player.Badges.Add(new BadgeEntity
                {
                    Id = rule.BadgeId.Value,
                });
            }
        }

        // Save everything
        await _playerRepo.UpdateAsync(player);
        await _playerRepo.SaveChangesAsync();
        await _eventRepo.SaveChangesAsync();

        return new EventCreatedResponseDto
        {
            Status = "created",
            Url = $"/events/{ev.Id}",
            Id = ev.Id
        };
    }
}
