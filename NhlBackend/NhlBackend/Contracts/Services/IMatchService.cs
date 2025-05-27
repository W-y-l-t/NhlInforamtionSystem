using NhlBackend.Models.DataTransferObjects;

namespace NhlBackend.Contracts.Services;

public interface IMatchesService
{
    Task<MatchDetailsDto> GetMatchDetailsAsync(int matchId, CancellationToken cancellationToken);
}