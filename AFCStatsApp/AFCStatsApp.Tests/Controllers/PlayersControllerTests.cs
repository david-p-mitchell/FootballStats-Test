using AFCStatsApp.Controllers;
using AFCStatsApp.Interfaces.Services;
using AFCStatsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Numerics;

namespace AFCStatsApp.Tests.Controllers;

public class PlayersControllerTests
{
    private readonly Mock<IPlayerService> _serviceMock;
    private readonly PlayersController _controller;

    public PlayersControllerTests()
    {
        _serviceMock = new Mock<IPlayerService>();
        _controller = new PlayersController(_serviceMock.Object);
    }

    [Fact]
    public void Index_ReturnsViewResult()
    {
        var result = _controller.Index();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithPlayers()
    {
        var players = new List<PlayerModel>
        {
            new PlayerModel { PlayerId = 1, PlayerName = "John", Position = PositionEnum.Forward, JerseyNumber = 10 }
        };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(players);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(players, okResult.Value);
    }

    [Fact]
    public async Task Add_InvalidModel_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("PlayerName", "Required");

        var result = await _controller.Add(new PlayerModel { PlayerName = "", Position = PositionEnum.Midfielder, JerseyNumber = 5 });

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Add_WithPlayerId_ReturnsBadRequest()
    {
        var player = new PlayerModel { PlayerId = 1, PlayerName = "John", Position = PositionEnum.Defender, JerseyNumber = 10 };

        var result = await _controller.Add(player);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("A new player cannot have a Player Id", badRequest.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    public async Task Add_InvalidJerseyNumber_ReturnsBadRequest(short jerseyNumber)
    {
        var player = new PlayerModel { PlayerName = "John", Position = PositionEnum.Goalkeeper, JerseyNumber = jerseyNumber };

        var result = await _controller.Add(player);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Jersey number must be 1-99", badRequest.Value);
    }

    [Fact]
    public async Task Add_ValidPlayer_ReturnsOk()
    {
        var player = new PlayerModel { PlayerName = "John", Position = PositionEnum.Forward, JerseyNumber = 9 };
        _serviceMock.Setup(s => s.AddAsync(player)).ReturnsAsync(player);

        var result = await _controller.Add(player);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(player, okResult.Value);
    }

    [Fact]
    public async Task Update_InvalidModel_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("PlayerName", "Required");

        var player = new PlayerModel { PlayerId = 1, PlayerName = "", Position = PositionEnum.Midfielder, JerseyNumber = 10 };

        var result = await _controller.Update(player);

        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    public async Task Update_InvalidJerseyNumber_ReturnsBadRequest(short jerseyNumber)
    {
        var player = new PlayerModel { PlayerId = 1, PlayerName = "Mikel", Position = PositionEnum.Defender, JerseyNumber = jerseyNumber };

        var result = await _controller.Update(player);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Jersey number must be 1-99", badRequest.Value);
    }

    [Fact]
    public async Task Update_ValidPlayer_ReturnsOk()
    {
        var player = new PlayerModel { PlayerId = 1, PlayerName = "David", Position = PositionEnum.Goalkeeper, JerseyNumber = 1 };
        _serviceMock.Setup(s => s.UpdateAsync(player)).ReturnsAsync(player);

        var result = await _controller.Update(player);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(player, okResult.Value);
    }

    [Fact]
    public async Task Delete_NonExistentPlayer_ReturnsNotFound()
    {
        // Arrange
        var mockService = new Mock<IPlayerService>();
        mockService.Setup(s => s.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);

        var controller = new PlayersController(mockService.Object);

        // Act
        var result = await controller.Delete(999); // 999 doesn't exist

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Player not found", notFoundResult.Value);

        mockService.Verify(s => s.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Delete_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var mockService = new Mock<IPlayerService>();
        var controller = new PlayersController(mockService.Object);
        controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
        mockService.Verify(s => s.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
