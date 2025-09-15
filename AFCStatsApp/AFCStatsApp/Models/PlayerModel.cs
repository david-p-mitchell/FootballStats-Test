namespace AFCStatsApp.Models
{
    public record PlayerModel
    {
        public int PlayerId { get; set; }
        public required string PlayerName { get; set; }

        public required PositionEnum Position { get; set; } 
        public required short JerseyNumber { get; set; }
        public int GoalsScored { get; set; } = 0;
    }
}
