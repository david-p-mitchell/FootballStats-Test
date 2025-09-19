using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AFCStatsApp.Models;

public record PlayerModel
{
    public int PlayerId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Player name cannot be empty.")]
    public required string PlayerName { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required PositionEnum Position { get; set; }
    
    [Range(1,99)]
    public required byte JerseyNumber { get; set; }

    public int GoalsScored { get; set; } = 0;
}
