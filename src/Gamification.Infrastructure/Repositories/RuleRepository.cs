using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;
using Gamification.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure.Repositories;

/// <inheritdoc />
public class RuleRepository : IRuleRepository
{
    private readonly GamificationDbContext _db;

    public RuleRepository(GamificationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public Task<List<RuleEntity>> GetAllAsync(int applicationId)
    {
        // Return all rules for the application, with their event type
        return _db.Rules
            .Include(r => r.EventType)
            .Where(r => r.ApplicationId == applicationId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public Task<RuleEntity?> GetByIdAsync(int applicationId, int ruleId)
    {
        // Return rule by id, with its event type
        return _db.Rules
            .Include(r => r.EventType)
            .FirstOrDefaultAsync(r => r.ApplicationId == applicationId && r.Id == ruleId);
    }

    /// <inheritdoc />
    public async Task AddAsync(RuleEntity entity)
    {
        // Add new rule
        await _db.Rules.AddAsync(entity);
    }

    /// <inheritdoc />
    public Task UpdateAsync(RuleEntity entity)
    {
        // Mark as modified
        _db.Rules.Update(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(RuleEntity entity)
    {
        // Remove rule
        _db.Rules.Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SaveChangesAsync()
    {
        // Persist changes
        return _db.SaveChangesAsync();
    }
}
