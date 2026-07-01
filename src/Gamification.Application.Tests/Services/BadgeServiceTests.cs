using Gamification.Application.DTOs.Badge;
using Gamification.Application.Interfaces;
using Gamification.Application.Services;
using Gamification.Domain.Entities;
using Moq;

namespace Gamification.Application.Tests.Services;

public class BadgeServiceTests
{
    private readonly Mock<IApplicationRepository> _appRepoMock;
    private readonly Mock<IBadgeRepository> _badgeRepoMock;
    private readonly BadgeService _service;

    public BadgeServiceTests()
    {
        _appRepoMock = new Mock<IApplicationRepository>();
        _badgeRepoMock = new Mock<IBadgeRepository>();
        _service = new BadgeService(_appRepoMock.Object, _badgeRepoMock.Object);
    }

    // -------------------------------------------------------------
    // GET ALL
    // -------------------------------------------------------------

    [Fact]
    public async Task GetAllAsync_ShouldReturnBadges_WhenCredentialsAreValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var badges = new List<BadgeEntity>
        {
            new BadgeEntity { Id = 1, Name = "Gold", Description = "Gold badge", Icon = "gold.png" },
            new BadgeEntity { Id = 2, Name = "Silver", Description = "Silver badge", Icon = "silver.png" }
        };

        _badgeRepoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(badges);

        // Act
        var result = await _service.GetAllAsync("key", "pass");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Gold", result[0].Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenCredentialsAreInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        // Act
        var result = await _service.GetAllAsync("bad", "creds");

        // Assert
        Assert.Empty(result);
    }

    // -------------------------------------------------------------
    // GET BY ID
    // -------------------------------------------------------------

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBadge_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var badge = new BadgeEntity
        {
            Id = 10,
            Name = "Gold",
            Description = "Gold badge",
            Icon = "gold.png"
        };

        _badgeRepoMock.Setup(r => r.GetByIdAsync(1, 10)).ReturnsAsync(badge);

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Gold", result!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenBadgeNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _badgeRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                      .ReturnsAsync((BadgeEntity?)null);

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        // Act
        var result = await _service.GetByIdAsync("bad", "creds", 1);

        // Assert
        Assert.Null(result);
    }

    // -------------------------------------------------------------
    // CREATE
    // -------------------------------------------------------------

    [Fact]
    public async Task CreateAsync_ShouldCreateBadge_WhenCredentialsValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var dto = new CreateBadgeDto
        {
            Name = "Gold",
            Description = "Gold badge",
            Icon = "gold.png"
        };

        // Act
        var result = await _service.CreateAsync("key", "pass", dto);

        // Assert
        Assert.Equal("created", result.Status);
        _badgeRepoMock.Verify(r => r.AddAsync(It.IsAny<BadgeEntity>()), Times.Once);
        _badgeRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        var dto = new CreateBadgeDto();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync("bad", "creds", dto));
    }

    // -------------------------------------------------------------
    // UPDATE
    // -------------------------------------------------------------

    [Fact]
    public async Task UpdateAsync_ShouldUpdateBadge_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var badge = new BadgeEntity { Id = 10, Name = "Old" };
        _badgeRepoMock.Setup(r => r.GetByIdAsync(1, 10)).ReturnsAsync(badge);

        var dto = new UpdateBadgeDto
        {
            Name = "New",
            Description = "Updated",
            Icon = "new.png"
        };

        // Act
        var result = await _service.UpdateAsync("key", "pass", 10, dto);

        // Assert
        Assert.True(result);
        _badgeRepoMock.Verify(r => r.UpdateAsync(badge), Times.Once);
        _badgeRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenBadgeNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _badgeRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                      .ReturnsAsync((BadgeEntity?)null);

        var dto = new UpdateBadgeDto();

        // Act
        var result = await _service.UpdateAsync("key", "pass", 99, dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        var dto = new UpdateBadgeDto();

        // Act
        var result = await _service.UpdateAsync("bad", "creds", 1, dto);

        // Assert
        Assert.False(result);
    }

    // -------------------------------------------------------------
    // DELETE
    // -------------------------------------------------------------

    [Fact]
    public async Task DeleteAsync_ShouldDeleteBadge_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var badge = new BadgeEntity { Id = 10 };
        _badgeRepoMock.Setup(r => r.GetByIdAsync(1, 10)).ReturnsAsync(badge);

        // Act
        var result = await _service.DeleteAsync("key", "pass", 10);

        // Assert
        Assert.True(result);
        _badgeRepoMock.Verify(r => r.DeleteAsync(badge), Times.Once);
        _badgeRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenBadgeNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _badgeRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                      .ReturnsAsync((BadgeEntity?)null);

        // Act
        var result = await _service.DeleteAsync("key", "pass", 99);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        // Act
        var result = await _service.DeleteAsync("bad", "creds", 1);

        // Assert
        Assert.False(result);
    }
}
