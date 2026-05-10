using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;
using Gamification.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure.Repositories;

/// <inheritdoc />
public class ApplicationRepository : IApplicationRepository
{
    private readonly GamificationDbContext _db;

    public ApplicationRepository(GamificationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public Task<ApplicationEntity?> GetByCredentialsAsync(string apiKey, string apiPassword)
    {
        // Query by API credentials
        return _db.Applications
            .FirstOrDefaultAsync(a => a.ApiKey == apiKey && a.ApiPassword == apiPassword);
    }

    /// <inheritdoc />
    public Task<ApplicationEntity?> GetByIdAsync(int id)
    {
        // Query by ID
        return _db.Applications.FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <inheritdoc />
    public async Task AddAsync(ApplicationEntity entity)
    {
        // Add new entity to EF Core tracking
        await _db.Applications.AddAsync(entity);
    }

    /// <inheritdoc />
    public Task UpdateAsync(ApplicationEntity entity)
    {
        // Mark entity as modified
        _db.Applications.Update(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(ApplicationEntity entity)
    {
        // Remove entity from tracking
        _db.Applications.Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SaveChangesAsync()
    {
        // Persist changes to database
        return _db.SaveChangesAsync();
    }

    /// <inheritdoc />
    public Task<bool> ApiKeyExistsAsync(string apiKey)
    {
        // Check if any application uses this apiKey
        return _db.Applications.AnyAsync(a => a.ApiKey == apiKey);
    }

    /// <inheritdoc />
    public Task<bool> ApiPasswordExistsAsync(string apiPassword)
    {
        // Check if any application uses this apiPassword
        return _db.Applications.AnyAsync(a => a.ApiPassword == apiPassword);
    }
}
