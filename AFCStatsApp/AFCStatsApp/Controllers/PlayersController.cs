using AFCStatsApp.Interfaces.Services;
using AFCStatsApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AFCStatsApp.Controllers;

public class PlayersController(IPlayerService _service) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var players = await _service.GetAllAsync();
        return Ok(players);
    }

    [HttpPost]
    public async Task<IActionResult> Add(PlayerModel player)
    {
        if (!ModelState.IsValid) return BadRequest();
        if (HasPlayerId(player)) return BadRequest("A new player cannot have a Player Id");
        if (!HasValidJerseyNumber(player)) return BadRequest("Jersey number must be 1-99");

        var newPlayer = await _service.AddAsync(player);
        return Ok(newPlayer);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PlayerModel player)
    {
        if (!ModelState.IsValid) return BadRequest();
        if (!HasValidJerseyNumber(player)) return BadRequest("Jersey number must be 1-99");

        var newPlayer = await _service.UpdateAsync(player);
        return Ok(newPlayer);
    }

    [HttpDelete("{playerid}")]
    public async Task<IActionResult> Delete(int playerId)
    {
        if (!ModelState.IsValid) return BadRequest();
        
        var deletedPlayer = await _service.DeleteAsync(playerId);
        if (!deletedPlayer) return NotFound("Player not found");
        return NoContent();
    }

    private bool HasValidJerseyNumber(PlayerModel player) => player.JerseyNumber >= 1 && player.JerseyNumber <= 99;
    private bool HasPlayerId(PlayerModel player) => player.PlayerId > 0;
}
