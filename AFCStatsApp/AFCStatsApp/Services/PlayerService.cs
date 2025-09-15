using AFCStatsApp.Interfaces.Repositories;
using AFCStatsApp.Interfaces.Services;
using AFCStatsApp.Models;

namespace AFCStatsApp.Services;

public class PlayerService(IPlayerRepository playerRepository) : IPlayerService
{
    public readonly IPlayerRepository _playerRepository = playerRepository;

    public async Task<IEnumerable<PlayerModel>> GetAllAsync() => await _playerRepository.GetAllAsync();

    public async Task<PlayerModel> AddAsync(PlayerModel newPlayer) => await _playerRepository.AddAsync(newPlayer);

    public async Task<bool> DeleteAsync(int playerId)
    {
        var player = await _playerRepository.GetByIdAsync(playerId);
        if (player == null) return false;
        await _playerRepository.DeleteAsync(playerId);
        return true;
    }

    public async Task<PlayerModel> UpdateAsync(PlayerModel playerToBeUpdated) => await _playerRepository.UpdateAsync(playerToBeUpdated);
}
