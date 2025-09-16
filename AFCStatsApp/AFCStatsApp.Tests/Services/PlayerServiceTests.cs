using AFCStatsApp.Interfaces.Repositories;
using AFCStatsApp.Models;
using AFCStatsApp.Services;
using Moq;
using System.Numerics;

namespace AFCStatsApp.Tests.Services;

public class PlayerServiceTests
{
    private readonly Mock<IPlayerRepository> _mockRepo;
    private readonly PlayerService _service;

    public PlayerServiceTests()
    {
        _mockRepo = new Mock<IPlayerRepository>();
        _service = new PlayerService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPlayers()
    {
        // Arrange
        var players = new List<PlayerModel>
        {
            new() { PlayerId = 1, PlayerName = "Ben White", Position = PositionEnum.Defender, JerseyNumber = 4 },
            new() { PlayerId = 2, PlayerName = "David Raya", Position = PositionEnum.Goalkeeper, JerseyNumber = 1 }
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(players);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(players.Count, ((List<PlayerModel>)result).Count);
        Assert.Collection(players, player =>
        {
            Assert.Equal(1,player.PlayerId);
            Assert.Equal("Ben White", player.PlayerName);

            Assert.Equal(PositionEnum.Defender, player.Position);
            Assert.Equal(4, player.JerseyNumber);
            Assert.Equal(0, player.GoalsScored);
        },
        player =>
        {
            Assert.Equal(2, player.PlayerId);
            Assert.Equal("David Raya", player.PlayerName);

            Assert.Equal(PositionEnum.Goalkeeper, player.Position);
            Assert.Equal(1, player.JerseyNumber);
            Assert.Equal(0, player.GoalsScored);
        });
    }

    [Fact]
    public async Task AddAsync_ShouldReturnAddedPlayer()
    {
        
        var newPlayer = new PlayerModel { PlayerName = "Ben", Position = PositionEnum.Defender, JerseyNumber = 4 };
        _mockRepo.Setup(r => r.AddAsync(newPlayer)).ReturnsAsync(newPlayer);

        var result = await _service.AddAsync(newPlayer);

        Assert.Equal(newPlayer.PlayerName, result.PlayerName);
    }

    [Fact]
    public async Task DeleteAsync_PlayerExists_ShouldReturnTrue()
    {
        // Arrange
        var player = new PlayerModel { PlayerId = 1, PlayerName = "Ben", JerseyNumber = 4, Position= PositionEnum.Defender };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(player);
        _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);
        _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedPlayer()
    {
        // Arrange
        var updatedPlayer = new PlayerModel { PlayerId = 1, PlayerName = "Updated Player", Position = PositionEnum.Goalkeeper, JerseyNumber = 1 };
        _mockRepo.Setup(r => r.UpdateAsync(updatedPlayer)).ReturnsAsync(updatedPlayer);

        // Act
        var result = await _service.UpdateAsync(updatedPlayer);

        // Assert
        Assert.Equal(updatedPlayer.PlayerName, result.PlayerName);
    }

    [Fact]
    public async Task DeleteAsync_PlayerDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((PlayerModel?)null);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.False(result);
        _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
