using Gamification.Application.DTOs.Event;
using Gamification.Application.Exceptions;
using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;

namespace Gamification.Application.Services;

/// <summary>
/// Implements operations for managing events.
/// Everything that has to be monitored by the gamification platform is recorded
/// as an event and has to be posted to the gamification API.
/// </summary>
public class EventService : IEventService
{
    private readonly IApplicationRepository _appRepo;
    private readonly IEventRepository _eventRepo;
    private readonly IPlayerRepository _playerRepo;
    private readonly IRuleRepository _ruleRepo;
    private readonly IEventTypeRepository _eventTypeRepo;

    public EventService(
        IApplicationRepository appRepo,
        IEventRepository eventRepo,
        IPlayerRepository playerRepo,
        IRuleRepository ruleRepo,
        IEventTypeRepository eventTypeRepo)
    {
        _appRepo = appRepo;
        _eventRepo = eventRepo;
        _playerRepo = playerRepo;
        _ruleRepo = ruleRepo;
        _eventTypeRepo = eventTypeRepo;
    }

    /// <inheritdoc />
    public async Task<List<EventListItemDto>> GetAllAsync(string apiKey, string apiPassword)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        var events = await _eventRepo.GetAllAsync(app.Id);

        return events.Select(e => new EventListItemDto
        {
            Id = e.Id,
            PlayerId = e.PlayerId,
            EventTypeId = e.EventTypeId,
            EventTypeName = e.EventType.Name,
            Timestamp = e.Timestamp
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<EventDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int eventId)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        var ev = await _eventRepo.GetByIdAsync(app.Id, eventId);
        if (ev is null)
        {
            return null;
        }

        return new EventDetailsDto
        {
            Id = ev.Id,
            PlayerId = ev.PlayerId,
            EventTypeId = ev.EventTypeId,
            EventTypeName = ev.EventType.Name,
            Timestamp = ev.Timestamp
        };
    }

    /// <inheritdoc />
    public async Task<EventCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateEventDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Load player
        var player = await _playerRepo.GetByIdAsync(app.Id, dto.PlayerId);
        if (player is null)
        {
            throw new InvalidOperationException("Player not found.");
        }

        // Load the event type referenced by the event
        var eventType = await _eventTypeRepo.GetByIdAsync(app.Id, dto.EventTypeId);
        if (eventType is null)
        {
            throw new InvalidOperationException("Event type not found.");
        }

        // Create event
        var ev = new EventEntity
        {
            PlayerId = dto.PlayerId,
            EventTypeId = dto.EventTypeId,
            Timestamp = dto.Timestamp,
            ApplicationId = app.Id
        };

        await _eventRepo.AddAsync(ev);

        // Apply matching rules (rules reference the same event type id)
        var rules = await _ruleRepo.GetAllAsync(app.Id);

        foreach (var rule in rules.Where(r => r.EventTypeId == dto.EventTypeId))
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

        // Save everything in one atomic save: the event insert and the rule
        // awards (points and badges) are persisted together or not at all
        await _eventRepo.SaveChangesAsync();

        return new EventCreatedResponseDto
        {
            Status = "created",
            Url = $"/events/{ev.Id}",
            Id = ev.Id
        };
    }
}
