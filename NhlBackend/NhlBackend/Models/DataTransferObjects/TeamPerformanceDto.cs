namespace NhlBackend.Models.DataTransferObjects;

public record TeamPerformanceDto(
    int TeamId,
    string Season,
    int GamesPlayed,
    int Wins,
    int Losses,
    int GoalsFor,
    int GoalsAgainst
);