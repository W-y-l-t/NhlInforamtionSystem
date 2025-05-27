namespace NhlBackend.Models.DataTransferObjects;

public record PlayerSniperDto(
    int PlayerId,
    string Player,
    string? Team,
    int GoalsScored);