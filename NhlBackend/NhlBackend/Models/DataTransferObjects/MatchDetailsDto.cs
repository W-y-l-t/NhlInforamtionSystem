namespace NhlBackend.Models.DataTransferObjects;

public record MatchDetailsDto(
    int MatchId,
    DateTime MatchDate,
    string HomeTeam,
    string AwayTeam,
    string Arena,
    IReadOnlyCollection<GoalDetailsDto> Goals,
    IReadOnlyCollection<string> Referees
);