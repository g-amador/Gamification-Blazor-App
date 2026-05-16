using Gamification.Application.DTOs.Player;
using Gamification.Application.Interfaces;
using Gamification.Application.Services;
using Gamification.Domain.Entities;
using Moq;

namespace Gamification.Application.Tests.Services;

public class PlayerServiceTests
{
    private readonly Mock<IApplicationRepository> _appRepoMock;
    private readonly Mock<IPlayerRepository> _playerRepoMock;
    private readonly PlayerService _service;

    public PlayerServiceTests()
    {
        _appRepoMock = new Mock<IApplicationRepository>();
        _playerRepoMock = new Mock<IPlayerRepository>();
        _service = new PlayerService(_appRepoMock.Object, _playerRepoMock.Object);
    }

    // -------------------------------------------------------------
    // GET ALL
    // -------------------------------------------------------------

    [Fact]
    public async Task GetAllAsync_ShouldReturnPlayers_WhenCredentialsAreValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var players = new List<PlayerEntity>
        {
            new PlayerEntity { Id = 1, FirstName = "John", LastName = "Doe", Email = "a@a.com", NumberOfPoints = 10 },
            new PlayerEntity { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "b@b.com", NumberOfPoints = 20 }
        };

        _playerRepoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(players);

        // Act
        var result = await _service.GetAllAsync("key", "pass");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("John", result[0].FirstName);
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
    public async Task GetByIdAsync_ShouldReturnPlayer_WhenCredentialsAndIdAreValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var player = new PlayerEntity
        {
            Id = 5,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            NumberOfPoints = 50,
            Badges = new List<BadgeEntity>()
        };

        _playerRepoMock.Setup(r => r.GetByIdAsync(1, 5)).ReturnsAsync(player);

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result!.FirstName);
        Assert.Equal(50, result.NumberOfPoints);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPlayerDoesNotExist()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _playerRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                       .ReturnsAsync((PlayerEntity?)null);

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
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
    public async Task CreateAsync_ShouldCreatePlayer_WhenCredentialsAreValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var dto = new CreatePlayerDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com"
        };

        // Act
        var result = await _service.CreateAsync("key", "pass", dto);

        // Assert
        Assert.Equal("created", result.Status);
        _playerRepoMock.Verify(r => r.AddAsync(It.IsAny<PlayerEntity>()), Times.Once);
        _playerRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCredentialsAreInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        var dto = new CreatePlayerDto();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync("bad", "creds", dto));
    }

    // -------------------------------------------------------------
    // UPDATE
    // -------------------------------------------------------------

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePlayer_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var player = new PlayerEntity { Id = 10, FirstName = "Old" };
        _playerRepoMock.Setup(r => r.GetByIdAsync(1, 10)).ReturnsAsync(player);

        var dto = new UpdatePlayerDto
        {
            FirstName = "New",
            LastName = "Name",
            Email = "new@test.com"
        };

        // Act
        var result = await _service.UpdateAsync("key", "pass", 10, dto);

        // Assert
        Assert.True(result);
        _playerRepoMock.Verify(r => r.UpdateAsync(player), Times.Once);
        _playerRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenPlayerNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _playerRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                       .ReturnsAsync((PlayerEntity?)null);

        var dto = new UpdatePlayerDto();

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

        var dto = new UpdatePlayerDto();

        // Act
        var result = await _service.UpdateAsync("bad", "creds", 1, dto);

        // Assert
        Assert.False(result);
    }

    // -------------------------------------------------------------
    // DELETE
    // -------------------------------------------------------------

    [Fact]
    public async Task DeleteAsync_ShouldDeletePlayer_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var player = new PlayerEntity { Id = 10 };
        _playerRepoMock.Setup(r => r.GetByIdAsync(1, 10)).ReturnsAsync(player);

        // Act
        var result = await _service.DeleteAsync("key", "pass", 10);

        // Assert
        Assert.True(result);
        _playerRepoMock.Verify(r => r.DeleteAsync(player), Times.Once);
        _playerRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenPlayerNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _playerRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                       .ReturnsAsync((PlayerEntity?)null);

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
