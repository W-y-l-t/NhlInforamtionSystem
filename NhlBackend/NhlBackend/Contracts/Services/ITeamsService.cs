using NhlBackend.Models.DataTransferObjects;
using NhlBackend.Models.Types;

namespace NhlBackend.Contracts.Services;

public interface ITeamsService
{
    Task<IReadOnlyCollection<TeamDto>> GetAllTeamsAsync(CancellationToken cancellationToken);
    Task<TeamPerformanceDto> GetTeamPerformanceAsync(int teamId, YearOnly season, CancellationToken cancellationToken);
}