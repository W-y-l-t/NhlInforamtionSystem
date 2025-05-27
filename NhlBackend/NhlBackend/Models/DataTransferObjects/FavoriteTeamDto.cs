namespace NhlBackend.Models.DataTransferObjects;

public record FavoriteTeamDto(
    int TeamId,
    string FullName,
    string ShortName,
    string? ArenaName,
    int? ArenaCapacity,
    string? DivisionName,
    string? ConferenceName
);