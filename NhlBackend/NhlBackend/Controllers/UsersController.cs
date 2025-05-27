using Microsoft.AspNetCore.Mvc;
using NhlBackend.Contracts.Services;
using NhlBackend.Models.DataTransferObjects;

namespace NhlBackend.Controllers;

[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet("tickets-of/{userId}")]
    public async Task<ActionResult<IReadOnlyCollection<TicketDto>>> GetTicketsOfUserAsync(
        [FromRoute] long userId,
        CancellationToken cancellationToken = default)
    {
        var response = await _usersService.GetTicketsOfUserAsync(userId, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpGet("favorites/{userId}")]
    public async Task<ActionResult<IReadOnlyCollection<FavoriteTeamDto>>> GetFavoriteTeamsAsync(
        [FromRoute] long userId,
        CancellationToken cancellationToken = default)
    {
        var response = 
            await _usersService.GetFavoriteTeamsAsync(userId, cancellationToken);
        
        return Ok(response);
    }
}