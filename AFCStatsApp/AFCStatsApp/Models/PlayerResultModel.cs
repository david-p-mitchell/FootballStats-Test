namespace AFCStatsApp.Models;

public class ResultModel
{
    public bool Success { get; set; } = false;
}

public class PlayerResultModel : ResultModel
{
    public PlayerResultModel() { Success = true; }
    public required PlayerModel Player { get; set; }
}
