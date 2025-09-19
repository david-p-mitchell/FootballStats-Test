namespace AFCStatsApp.Models;
public record Area(int Id, string Name, string Code, string Flag);

public record Team(int Id, string Name, string ShortName, string Tla, string Crest);

public record Score(string Winner, string Duration, FullTimeScore FullTime, HalfTimeScore HalfTime);

public record FullTimeScore(int? Home, int? Away);

public record HalfTimeScore(int? Home, int? Away);

public record Match(DateTime UtcDate, string Status, Area Area, Team HomeTeam, Team AwayTeam, Score Score);

public record MatchStoreModel(List<Match> Matches);
