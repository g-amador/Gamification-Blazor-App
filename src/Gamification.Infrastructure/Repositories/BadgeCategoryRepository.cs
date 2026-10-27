using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;
using Gamification.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure.Repositories;

/// <inheritdoc />
public class BadgeCategoryRepository : IBadgeCategoryRepository
{
    private readonly GamificationDbContext _db;

    public BadgeCategoryRepository(GamificationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public Task<List<BadgeCategoryEntity>> GetAllAsync(int applicationId)
    {
        // Return all badge categories for the application
        return _db.BadgeCategories
            .Where(c => c.ApplicationId == applicationId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public Task<BadgeCategoryEntity?> GetByIdAsync(int applicationId, int categoryId)
    {
        // Return badge category by id
        return _db.BadgeCategories
            .FirstOrDefaultAsync(c => c.ApplicationId == applicationId && c.Id == categoryId);
    }

    /// <inheritdoc />
    public async Task AddAsync(BadgeCategoryEntity entity)
    {
        // Add new badge category to EF Core tracking
        await _db.BadgeCategories.AddAsync(entity);
    }

    /// <inheritdoc />
    public Task UpdateAsync(BadgeCategoryEntity entity)
    {
        // Mark entity as modified
        _db.BadgeCategories.Update(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SaveChangesAsync()
    {
        // Persist changes to database
        return _db.SaveChangesAsync();
    }

    /// <inheritdoc />
    public Task<bool> NameExistsAsync(int applicationId, string name)
    {
        // Check if any badge category of the application uses this name
        return _db.BadgeCategories
            .AnyAsync(c => c.ApplicationId == applicationId && c.Name == name);
    }
}
