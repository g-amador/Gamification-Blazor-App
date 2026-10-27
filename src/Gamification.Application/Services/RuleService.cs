using Gamification.Application.DTOs.Rule;
using Gamification.Application.Exceptions;
using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;

namespace Gamification.Application.Services;

/// <summary>
/// Implements operations for managing rules.
/// A rule defines when badges or points should be granted to a player.
/// Many rules can be created for the same event type.
/// </summary>
public class RuleService : IRuleService
{
    private readonly IApplicationRepository _appRepo;
    private readonly IRuleRepository _ruleRepo;
    private readonly IEventTypeRepository _eventTypeRepo;

    public RuleService(IApplicationRepository appRepo, IRuleRepository ruleRepo, IEventTypeRepository eventTypeRepo)
    {
        _appRepo = appRepo;
        _ruleRepo = ruleRepo;
        _eventTypeRepo = eventTypeRepo;
    }

    /// <inheritdoc />
    public async Task<List<RuleListItemDto>> GetAllAsync(string apiKey, string apiPassword)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Load rules
        var rules = await _ruleRepo.GetAllAsync(app.Id);

        // Map to DTOs
        return rules.Select(r => new RuleListItemDto
        {
            Id = r.Id,
            BadgeId = r.BadgeId,
            NumberOfPoints = r.NumberOfPoints,
            EventTypeId = r.EventTypeId,
            EventTypeName = r.EventType.Name
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<RuleDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int ruleId)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Load rule
        var rule = await _ruleRepo.GetByIdAsync(app.Id, ruleId);
        if (rule is null)
        {
            return null;
        }

        // Map to DTO
        return new RuleDetailsDto
        {
            Id = rule.Id,
            BadgeId = rule.BadgeId,
            NumberOfPoints = rule.NumberOfPoints,
            EventTypeId = rule.EventTypeId,
            EventTypeName = rule.EventType.Name
        };
    }

    /// <inheritdoc />
    public async Task<RuleCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateRuleDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // The rule must reference an event type of the application
        var eventType = await _eventTypeRepo.GetByIdAsync(app.Id, dto.EventTypeId);
        if (eventType is null)
        {
            throw new InvalidOperationException("Event type not found.");
        }

        // Create entity
        var entity = new RuleEntity
        {
            BadgeId = dto.BadgeId,
            NumberOfPoints = dto.NumberOfPoints,
            EventTypeId = dto.EventTypeId,
            ApplicationId = app.Id
        };

        // Save
        await _ruleRepo.AddAsync(entity);
        await _ruleRepo.SaveChangesAsync();

        return new RuleCreatedResponseDto
        {
            Status = "created",
            Url = $"/rules/{entity.Id}",
            Id = entity.Id
        };
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(string apiKey, string apiPassword, int ruleId, UpdateRuleDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Load rule
        var rule = await _ruleRepo.GetByIdAsync(app.Id, ruleId);
        if (rule is null)
        {
            return false;
        }

        // The rule must reference an event type of the application
        var eventType = await _eventTypeRepo.GetByIdAsync(app.Id, dto.EventTypeId);
        if (eventType is null)
        {
            throw new InvalidOperationException("Event type not found.");
        }

        // Update fields
        rule.BadgeId = dto.BadgeId;
        rule.NumberOfPoints = dto.NumberOfPoints;
        rule.EventTypeId = dto.EventTypeId;

        // Save
        await _ruleRepo.UpdateAsync(rule);
        await _ruleRepo.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string apiKey, string apiPassword, int ruleId)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Load rule
        var rule = await _ruleRepo.GetByIdAsync(app.Id, ruleId);
        if (rule is null)
        {
            return false;
        }

        // Delete
        await _ruleRepo.DeleteAsync(rule);
        await _ruleRepo.SaveChangesAsync();

        return true;
    }
}
