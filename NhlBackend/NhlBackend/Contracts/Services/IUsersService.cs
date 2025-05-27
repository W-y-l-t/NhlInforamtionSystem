using NhlBackend.Models.DataTransferObjects;

namespace NhlBackend.Contracts.Services;

public interface IUsersService
{
    Task<IReadOnlyCollection<TicketDto>> GetTicketsOfUserAsync(long userId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<FavoriteTeamDto>> GetFavoriteTeamsAsync(long userId, CancellationToken cancellationToken);
}