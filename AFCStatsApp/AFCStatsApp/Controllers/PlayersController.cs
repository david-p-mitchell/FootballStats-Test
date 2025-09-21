using AFCStatsApp.Interfaces.Services;
using AFCStatsApp.Models;
using AFCStatsApp.Models.Player;
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


    [HttpGet]
    public async Task<ActionResult> PlayerModal(int? playerId)
    {
        
        if (!playerId.HasValue)
        {
            var model = new PlayerModalProps { Show = true };
            return PartialView("_PlayerModal", model);
        }

        var players = await _playerService.GetAllAsync();
        var player = players.Where(playerFromStore => playerFromStore.PlayerId == playerId).FirstOrDefault();
        if (player != null) return PartialView("_PlayerModal", player);
        

        return NotFound();
        
    }


    [HttpPost("/api/players/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([FromBody]PlayerModel player)
    {
        if (!ModelState.IsValid || player == null) return BadRequest(new ErrorResultModel() { Errors = "Invalid player data" });
        if (HasPlayerId(player)) return BadRequest(new ErrorResultModel() { Errors = "A new player cannot have a Player Id" });
        if (!HasValidJerseyNumber(player)) return BadRequest(new ErrorResultModel() { Errors = "Jersey number must be 1-99" });
        try
        {
            if (await _playerService.ExistsByJerseyNumberAsync(player.JerseyNumber)) return BadRequest(new ErrorResultModel() { Errors = "Jersey number already in use" });

            var newPlayer = await _playerService.AddAsync(player);
            var personResult = new PlayerResultModel { Player = newPlayer, Success = true };
            return Ok(personResult);
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
            if (!HasValidJerseyNumber(player)) return BadRequest(new ErrorResultModel() { Errors = "Jersey number must be 1-99" });
            if (await _playerService.ExistsByJerseyNumberAsync(player.JerseyNumber, player.PlayerId)) return BadRequest(new ErrorResultModel() { Errors = "Updated Jersey number already in use by another player." });

            var newPlayer = await _playerService.UpdateAsync(player);
            var personResult = new PlayerResultModel { Player = newPlayer, Success = true };
            return Ok(personResult);
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
            var resultModel = new ResultModel() {  Success = true };
            return Ok(resultModel);
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
