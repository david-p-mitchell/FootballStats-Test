using AFCStatsApp.Models;

namespace AFCStatsApp.Interfaces.Repositories
{
    public interface IPlayerRepository
    {
        public IEnumerable<PlayerModel> GetAllPlayers();
        public PlayerModel GetPlayer(int playerId);
        public void AddPlayer(PlayerModel newPlayer);
        public void UpdatePlayer(PlayerModel playerToBeUpdated);
        public void DeletePlayer(PlayerModel playerToBeRemoved);
    }
}
