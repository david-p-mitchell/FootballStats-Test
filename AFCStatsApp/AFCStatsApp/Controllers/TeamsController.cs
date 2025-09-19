using AFCStatsApp.Interfaces.Services;
using AFCStatsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AFCStatsApp.Controllers
{
    public class TeamsController(IMatchesAPI matchesAPI, IMemoryCache cache) : Controller
    {
        private readonly IMatchesAPI _matchesApi = matchesAPI;
        private readonly IMemoryCache _cache = cache;

        public IActionResult Index() => View("Matches");

        [HttpGet("/api/teams/{teamId}/matches")]
        public async Task<IActionResult> Add(int teamId= 57)
        {
            
            if (teamId <= 0) return BadRequest(new ErrorResultModel() {  Errors = "Invalid Team Id"});
            
            try
            {
                var cacheKey = $"matches_{teamId}";
                var matchesStore = await GetMatchData(teamId, cacheKey);
                if (matchesStore == null) return Problem("There are no matches available");
                var returnedMatches = GetResultsAndSubsequentMatchesFromAllTeamMatches(matchesStore, 5);

                return Ok(returnedMatches);
            }
            catch (Exception)
            {
                return Problem("Failed to get Matches");
                throw;

            }
        }

        private async Task<MatchStoreModel?> GetMatchData(int teamId, string cacheKey)
        {
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var fetched = await _matchesApi.GetMatchesAsync(teamId);


                var upcomingMatch = GetNextMatch(fetched);

                if (upcomingMatch != null)
                {
                    var untilKickoff = upcomingMatch.UtcDate - DateTime.UtcNow;

                    if (untilKickoff > TimeSpan.Zero) entry.AbsoluteExpirationRelativeToNow = untilKickoff;
                    else entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                }
                else
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                }

                return fetched;
            });
        }

        private static List<Match> GetResultsAndSubsequentMatchesFromAllTeamMatches(MatchStoreModel matchesStore, int nextFixtureCount = 5)
        {
            var returnedMatches = new List<Match>();
            var completedMatchCount = matchesStore.Matches.Count(match => IsFinishedMatch(match));
            returnedMatches.AddRange(matchesStore.Matches.Take(completedMatchCount));
            returnedMatches.AddRange(matchesStore.Matches.Skip(completedMatchCount).Take(nextFixtureCount));

            return returnedMatches;
        }

        private static Match? GetNextMatch(MatchStoreModel matchStore) => matchStore.Matches
                        .Where(m => !IsFinishedMatch(m))
                        .OrderBy(m => m.UtcDate)
                        .FirstOrDefault();

        private static bool IsFinishedMatch(Match match) => match.Status == "FINISHED";
    }
    
}
