using Microsoft.AspNetCore.Mvc;
using NhlBackend.Contracts.Services;
using NhlBackend.Models.DataTransferObjects;
using NhlBackend.Models.Types;

namespace NhlBackend.Controllers;

[Route("players")]
public class PlayersController : ControllerBase
{
    private readonly IPlayersService _playersService;

    public PlayersController(IPlayersService playersService)
    {
        _playersService = playersService;
    }

    [HttpGet("of-team/{teamId}")]
    public async Task<ActionResult<IReadOnlyCollection<PlayerDto>>> GetPlayersOfTeamAsync(
        [FromRoute] int teamId, 
        CancellationToken cancellationToken = default)
    {
        var response = await _playersService.GetPlayersOfTeamAsync(teamId, cancellationToken);
        
        return Ok(response);
    }

    [HttpGet("awards-of/{playerId}")]
    public async Task<ActionResult<IReadOnlyCollection<AwardDto>>> GetPlayerAwardsAsync(
        [FromRoute] int playerId,
        CancellationToken cancellationToken = default)
    {
        var response = await _playersService.GetPlayerAwardsAsync(playerId, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpGet("top-snipers/{startYear}/{limit:int}")]
    public async Task<ActionResult<IReadOnlyCollection<PlayerSniperDto>>> GetTopSnipersAsync(
        [FromRoute] YearOnly startYear,
        [FromRoute] int limit = 10,
        CancellationToken cancellationToken = default)
    {
        var response = 
            await _playersService.GetTopSnipersAsync(startYear, limit, cancellationToken);
        
        return Ok(response);
    }
}