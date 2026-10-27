using Gamification.Application.Exceptions;
using Gamification.Application.Interfaces;
using Gamification.Application.Services;
using Gamification.Domain.Entities;
using Moq;

namespace Gamification.Application.Tests.Services;

public class LeaderboardServiceTests
{
    private readonly Mock<IApplicationRepository> _appRepoMock;
    private readonly Mock<IPlayerRepository> _playerRepoMock;
    private readonly LeaderboardService _service;

    public LeaderboardServiceTests()
    {
        _appRepoMock = new Mock<IApplicationRepository>();
        _playerRepoMock = new Mock<IPlayerRepository>();
        _service = new LeaderboardService(_appRepoMock.Object, _playerRepoMock.Object);
    }

    // -------------------------------------------------------------
    // GET LEADERBOARD
    // -------------------------------------------------------------

    [Fact]
    public async Task GetAsync_ShouldReturnLeaderboard_WhenCredentialsValid()
    {
        // Arrange
        var app = new ApplicationEntity
        {
            Id = 1,
            Name = "My App",
            Description = "Test Description"
        };

        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var players = new List<PlayerEntity>
        {
            new() { Id = 1, NumberOfPoints = 100 },
            new() { Id = 2, NumberOfPoints = 200 },
            new() { Id = 3, NumberOfPoints = 50 },
            new() { Id = 4, NumberOfPoints = 300 },
            new() { Id = 5, NumberOfPoints = 150 },
            new() { Id = 6, NumberOfPoints = 400 }
        };

        _playerRepoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(players);

        // Act
        var result = await _service.GetAsync("key", "pass");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("My App", result!.Name);
        Assert.Equal("Test Description", result.Description);

        Assert.Equal(5, result.Ranking.Count);
        Assert.Equal(400, result.Ranking[0].Points); // highest first
        Assert.Equal(300, result.Ranking[1].Points);
        Assert.Equal(200, result.Ranking[2].Points);
        Assert.Equal(150, result.Ranking[3].Points);
        Assert.Equal(100, result.Ranking[4].Points);
    }

    [Fact]
    public async Task GetAsync_ShouldThrowUnauthorizedException_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _service.GetAsync("bad", "creds"));
    }

    [Fact]
    public async Task GetAsync_ShouldReturnLessThanFive_WhenNotEnoughPlayers()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1, Name = "App", Description = "Desc" };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var players = new List<PlayerEntity>
        {
            new() { Id = 1, NumberOfPoints = 10 },
            new() { Id = 2, NumberOfPoints = 20 }
        };

        _playerRepoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(players);

        // Act
        var result = await _service.GetAsync("key", "pass");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result!.Ranking.Count);
        Assert.Equal(20, result.Ranking[0].Points);
        Assert.Equal(10, result.Ranking[1].Points);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnEmptyRanking_WhenNoPlayers()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1, Name = "App", Description = "Desc" };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _playerRepoMock.Setup(r => r.GetAllAsync(1))
                       .ReturnsAsync([]);

        // Act
        var result = await _service.GetAsync("key", "pass");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result!.Ranking);
    }
}
