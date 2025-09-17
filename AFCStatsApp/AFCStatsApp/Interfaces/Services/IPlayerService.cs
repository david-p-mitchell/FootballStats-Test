using AFCStatsApp.Models;
using System.Numerics;

namespace AFCStatsApp.Interfaces.Services
{
    public interface IPlayerService
    {
        public Task<IEnumerable<PlayerModel>> GetAllAsync();
        public Task<PlayerModel> AddAsync(PlayerModel newPlayer);
        public Task<PlayerModel> UpdateAsync(PlayerModel playerToBeUpdated);
        public Task<bool> DeleteAsync(int playerId);
        public Task<bool> ExistsByJerseyNumberAsync(byte jerseyNumber, int? playerId = null);
    }
}
