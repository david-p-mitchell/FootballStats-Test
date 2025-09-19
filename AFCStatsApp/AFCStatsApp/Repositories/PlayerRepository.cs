using AFCStatsApp.Db;
using AFCStatsApp.Interfaces.Repositories;
using AFCStatsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AFCStatsApp.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly AppDbContext _context;
    public PlayerRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get All Players
    /// </summary>
    /// <returns></returns>
    public async Task<List<PlayerModel>> GetAllAsync()
    {
        return await _context.Players.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Get a single player by their player id if exists
    /// </summary>
    /// <param name="id">Player Id</param>
    /// <returns></returns>
    public async Task<PlayerModel?> GetByIdAsync(int id)
    {
        return await _context.Players.FindAsync(id);
    }

    public async Task<PlayerModel> AddAsync(PlayerModel player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    // Update an existing player
    public async Task<PlayerModel> UpdateAsync(PlayerModel player)
    {
        try
        {
            var existingPlayer = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == player.PlayerId);
            existingPlayer!.PlayerName = player.PlayerName;
            existingPlayer!.JerseyNumber = player.JerseyNumber;
            existingPlayer!.Position = player.Position;
            existingPlayer!.GoalsScored = player.GoalsScored;
            // ... update any other fields

            await _context.SaveChangesAsync();

            return existingPlayer;
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    // Delete a player
    public async Task<bool> DeleteAsync(int id)
    {
        
        var player = await GetByIdAsync(id);
        if (player == null) return false;

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return true;
    }
}
