
using AFCStatsApp.Models;
using Refit;

namespace AFCStatsApp.Interfaces.Services;

public interface IMatchesAPI
{
    [Get("/teams/{teamId}/matches")]
    Task<MatchStoreModel> GetMatchesAsync(int teamId);
}
