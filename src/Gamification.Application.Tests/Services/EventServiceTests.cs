using Gamification.Application.DTOs.Event;
using Gamification.Application.Interfaces;
using Gamification.Application.Services;
using Gamification.Domain.Entities;
using Moq;

namespace Gamification.Application.Tests.Services;

public class EventServiceTests
{
    private readonly Mock<IApplicationRepository> _appRepoMock;
    private readonly Mock<IEventRepository> _eventRepoMock;
    private readonly Mock<IPlayerRepository> _playerRepoMock;
    private readonly Mock<IRuleRepository> _ruleRepoMock;
    private readonly EventService _service;

    public EventServiceTests()
    {
        _appRepoMock = new Mock<IApplicationRepository>();
        _eventRepoMock = new Mock<IEventRepository>();
        _playerRepoMock = new Mock<IPlayerRepository>();
        _ruleRepoMock = new Mock<IRuleRepository>();

        _service = new EventService(
            _appRepoMock.Object,
            _eventRepoMock.Object,
            _playerRepoMock.Object,
            _ruleRepoMock.Object
        );
    }

    // -------------------------------------------------------------
    // GET ALL
    // -------------------------------------------------------------

    [Fact]
    public async Task GetAllAsync_ShouldReturnEvents_WhenCredentialsValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var events = new List<EventEntity>
        {
            new EventEntity { Id = 1, PlayerId = 10, Type = "LOGIN", Timestamp = DateTime.UtcNow },
            new EventEntity { Id = 2, PlayerId = 11, Type = "PURCHASE", Timestamp = DateTime.UtcNow }
        };

        _eventRepoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(events);

        // Act
        var result = await _service.GetAllAsync("key", "pass");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("LOGIN", result[0].Type);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmpty_WhenCredentialsInvalid()
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
    public async Task GetByIdAsync_ShouldReturnEvent_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var ev = new EventEntity
        {
            Id = 5,
            PlayerId = 10,
            Type = "LOGIN",
            Timestamp = DateTime.UtcNow
        };

        _eventRepoMock.Setup(r => r.GetByIdAsync(1, 5)).ReturnsAsync(ev);

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("LOGIN", result!.Type);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEventNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _eventRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                      .ReturnsAsync((EventEntity?)null);

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
    // CREATE EVENT
    // -------------------------------------------------------------

    [Fact]
    public async Task CreateAsync_ShouldCreateEvent_AndApplyRules()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        var player = new PlayerEntity { Id = 10, NumberOfPoints = 0, Badges = [] };

        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _playerRepoMock.Setup(r => r.GetByIdAsync(1, 10))
                       .ReturnsAsync(player);

        var rules = new List<RuleEntity>
        {
            new RuleEntity { Id = 1, BadgeId = 5, NumberOfPoints = 50, OnEventType = "LOGIN", ApplicationId = 1 },
            new RuleEntity { Id = 2, BadgeId = null, NumberOfPoints = 20, OnEventType = "LOGIN", ApplicationId = 1 }
        };

        _ruleRepoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(rules);

        var dto = new CreateEventDto
        {
            PlayerId = 10,
            Type = "LOGIN",
            Timestamp = DateTime.UtcNow
        };

        // Act
        var result = await _service.CreateAsync("key", "pass", dto);

        // Assert
        Assert.Equal("created", result.Status);
        Assert.Equal(70, player.NumberOfPoints); // 50 + 20
        Assert.Single(player.Badges);
        Assert.Equal(5, player.Badges[0].Id);

        _eventRepoMock.Verify(r => r.AddAsync(It.IsAny<EventEntity>()), Times.Once);
        _eventRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _playerRepoMock.Verify(r => r.UpdateAsync(player), Times.Once);
        _playerRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotDuplicateBadge()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        var player = new PlayerEntity
        {
            Id = 10,
            NumberOfPoints = 0,
            Badges =
            [
                new BadgeEntity { Id = 5 }
            ]
        };

        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _playerRepoMock.Setup(r => r.GetByIdAsync(1, 10))
                       .ReturnsAsync(player);

        var rules = new List<RuleEntity>
        {
            new RuleEntity { Id = 1, BadgeId = 5, NumberOfPoints = 30, OnEventType = "LOGIN" }
        };

        _ruleRepoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(rules);

        var dto = new CreateEventDto
        {
            PlayerId = 10,
            Type = "LOGIN",
            Timestamp = DateTime.UtcNow
        };

        // Act
        var result = await _service.CreateAsync("key", "pass", dto);

        // Assert
        Assert.Equal("created", result.Status);
        Assert.Equal(30, player.NumberOfPoints);
        Assert.Single(player.Badges); // no duplicate badge
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenPlayerNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _playerRepoMock.Setup(r => r.GetByIdAsync(1, 10))
                       .ReturnsAsync((PlayerEntity?)null);

        var dto = new CreateEventDto { PlayerId = 10 };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync("key", "pass", dto));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        var dto = new CreateEventDto();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync("bad", "creds", dto));
    }
}
