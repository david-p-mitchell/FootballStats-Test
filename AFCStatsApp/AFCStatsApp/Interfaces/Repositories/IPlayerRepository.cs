using AFCStatsApp.Models;

namespace AFCStatsApp.Interfaces.Repositories
{
    public interface IPlayerRepository
    {
        public Task<List<PlayerModel>> GetAllAsync();
        public Task<PlayerModel?> GetByIdAsync(int playerId);
        public Task<PlayerModel> AddAsync(PlayerModel newPlayer);
        public Task<PlayerModel> UpdateAsync(PlayerModel playerToBeUpdated);
        public Task<bool> DeleteAsync(int playerId);
    }
}
