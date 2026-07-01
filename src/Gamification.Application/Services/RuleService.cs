using Gamification.Application.DTOs.Rule;
using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;

namespace Gamification.Application.Services;

/// <inheritdoc />
public class RuleService : IRuleService
{
    private readonly IApplicationRepository _appRepo;
    private readonly IRuleRepository _ruleRepo;

    public RuleService(IApplicationRepository appRepo, IRuleRepository ruleRepo)
    {
        _appRepo = appRepo;
        _ruleRepo = ruleRepo;
    }

    /// <inheritdoc />
    public async Task<List<RuleListItemDto>> GetAllAsync(string apiKey, string apiPassword)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            return [];
        }

        // Load rules
        var rules = await _ruleRepo.GetAllAsync(app.Id);

        // Map to DTOs
        return rules.Select(r => new RuleListItemDto
        {
            Id = r.Id,
            BadgeId = r.BadgeId,
            NumberOfPoints = r.NumberOfPoints,
            OnEventType = r.OnEventType
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<RuleDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int ruleId)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            return null;
        }

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
            OnEventType = rule.OnEventType
        };
    }

    /// <inheritdoc />
    public async Task<RuleCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateRuleDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        // Create entity
        var entity = new RuleEntity
        {
            BadgeId = dto.BadgeId,
            NumberOfPoints = dto.NumberOfPoints,
            OnEventType = dto.OnEventType,
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
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            return false;
        }

        // Load rule
        var rule = await _ruleRepo.GetByIdAsync(app.Id, ruleId);
        if (rule is null)
        {
            return false;
        }

        // Update fields
        rule.BadgeId = dto.BadgeId;
        rule.NumberOfPoints = dto.NumberOfPoints;
        rule.OnEventType = dto.OnEventType;

        // Save
        await _ruleRepo.UpdateAsync(rule);
        await _ruleRepo.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string apiKey, string apiPassword, int ruleId)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            return false;
        }

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
