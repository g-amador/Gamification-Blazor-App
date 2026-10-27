using Gamification.Application.DTOs.EventType;
using Gamification.Application.Exceptions;
using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;

namespace Gamification.Application.Services;

/// <summary>
/// Implements operations for managing event types.
/// All events that are recorded by the gamification API are associated with a type.
/// The types can be defined by the application using the API with the related API calls.
/// Then event types can be used in rules to define the conditions needed to obtain a badge.
/// </summary>
public class EventTypeService : IEventTypeService
{
    private readonly IApplicationRepository _appRepo;
    private readonly IEventTypeRepository _eventTypeRepo;

    public EventTypeService(IApplicationRepository appRepo, IEventTypeRepository eventTypeRepo)
    {
        _appRepo = appRepo;
        _eventTypeRepo = eventTypeRepo;
    }

    /// <inheritdoc />
    public async Task<List<EventTypeListItemDto>> GetAllAsync(string apiKey, string apiPassword)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Load event types
        var eventTypes = await _eventTypeRepo.GetAllAsync(app.Id);

        // Map to DTOs
        return eventTypes.Select(t => new EventTypeListItemDto
        {
            Id = t.Id,
            Name = t.Name
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<EventTypeDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int eventTypeId)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Load event type
        var eventType = await _eventTypeRepo.GetByIdAsync(app.Id, eventTypeId);
        if (eventType is null)
        {
            return null;
        }

        // Map to DTO
        return new EventTypeDetailsDto
        {
            Id = eventType.Id,
            Name = eventType.Name
        };
    }

    /// <inheritdoc />
    public async Task<EventTypeCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateEventTypeDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Ensure the event type name is unique within the application
        if (await _eventTypeRepo.NameExistsAsync(app.Id, dto.Name))
        {
            throw new InvalidOperationException("An event type with this name already exists for the application.");
        }

        // Create entity
        var entity = new EventTypeEntity
        {
            Name = dto.Name,
            ApplicationId = app.Id
        };

        // Save
        await _eventTypeRepo.AddAsync(entity);
        await _eventTypeRepo.SaveChangesAsync();

        return new EventTypeCreatedResponseDto
        {
            Status = "created",
            Url = $"/event-types/{entity.Id}",
            Id = entity.Id
        };
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(string apiKey, string apiPassword, int eventTypeId, UpdateEventTypeDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Load event type
        var eventType = await _eventTypeRepo.GetByIdAsync(app.Id, eventTypeId);
        if (eventType is null)
        {
            return false;
        }

        // Ensure the event type name is unique within the application (excluding itself)
        if (eventType.Name != dto.Name &&
            await _eventTypeRepo.NameExistsAsync(app.Id, dto.Name))
        {
            throw new InvalidOperationException("An event type with this name already exists for the application.");
        }

        // Update and save
        eventType.Name = dto.Name;
        await _eventTypeRepo.UpdateAsync(eventType);
        await _eventTypeRepo.SaveChangesAsync();

        return true;
    }
}
