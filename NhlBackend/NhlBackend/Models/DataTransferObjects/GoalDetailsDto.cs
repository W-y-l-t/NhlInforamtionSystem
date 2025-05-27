namespace NhlBackend.Models.DataTransferObjects;

public record GoalDetailsDto(
    int GoalId,
    string Scorer,
    string? Assist1,
    string? Assist2,
    string Team,
    int Period,
    int TimeInPeriodSec);