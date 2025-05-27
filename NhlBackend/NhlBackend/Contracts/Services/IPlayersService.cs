using NhlBackend.Models.DataTransferObjects;
using NhlBackend.Models.Types;

namespace NhlBackend.Contracts.Services;

public interface IPlayersService
{
    Task<IReadOnlyCollection<AwardDto>> GetPlayerAwardsAsync(int playerId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<PlayerDto>> GetPlayersOfTeamAsync(int teamId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<PlayerSniperDto>> GetTopSnipersAsync(
        YearOnly startYear,
        int limit,
        CancellationToken cancellationToken);
}