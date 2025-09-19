using AFCStatsApp.Interfaces.Services;
using AFCStatsApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AFCStatsApp.Controllers;

public class PlayersController(IPlayerService _playerService) : Controller
{
    public IActionResult Index() => View(); // pass the list to the view


    [HttpGet("api/players/getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var players = await _playerService.GetAllAsync();
            return Ok(players);
        }
        catch (Exception)
        {
            return Problem("Failed to get all Players");
            throw;

        }
    }

    [HttpPost("/api/players/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([FromBody]PlayerModel player)
    {
        if (!ModelState.IsValid || player == null) return BadRequest(new { errors = "Invalid player data" });
        if (HasPlayerId(player)) return BadRequest(new { errors = "A new player cannot have a Player Id" });
        if (!HasValidJerseyNumber(player)) return BadRequest(new { errors = "Jersey number must be 1-99" });
        try
        {
            if (await _playerService.ExistsByJerseyNumberAsync(player.JerseyNumber)) return BadRequest(new { errors = "Jersey number already in use" });

            var newPlayer = await _playerService.AddAsync(player);
            return Ok(new { success = true, player = newPlayer });
        }
        catch (Exception)
        {
            return Problem("Failed to add player");
            throw;

        }
    }

    [HttpPut("/api/players/update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update([FromBody] PlayerModel player)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest();
            if (!HasValidJerseyNumber(player)) return BadRequest("Jersey number must be 1-99");
            if (await _playerService.ExistsByJerseyNumberAsync(player.JerseyNumber, player.PlayerId)) return BadRequest(new { errors = "Updated Jersey number already in use by another player." });

            var newPlayer = await _playerService.UpdateAsync(player);
            return Ok(new { success = true, player = newPlayer });
        }
        catch(Exception)
        {
            return Problem("Failed to update player");
            throw;
        }
    }

    [HttpDelete("api/players/delete/{playerid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int playerId)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest();

            var deletedPlayer = await _playerService.DeleteAsync(playerId);
            if (!deletedPlayer) return NotFound("Player not found");
            return Ok(new { success = true });
        }
        catch (Exception)
        {
            return Problem("Failed to Delete Player");
            throw;
        }
    }

    private static bool HasValidJerseyNumber(PlayerModel player) => player.JerseyNumber >= 1 && player.JerseyNumber <= 99;
    private static bool HasPlayerId(PlayerModel player) => player.PlayerId > 0;
}
