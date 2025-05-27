using Microsoft.AspNetCore.Mvc;
using NhlBackend.Contracts.Services;
using NhlBackend.Models.DataTransferObjects;

namespace NhlBackend.Controllers;

[Route("matches")]
public class MatchesController : ControllerBase
{
    private readonly IMatchesService _matchesService;

    public MatchesController(IMatchesService matchesService)
    {
        _matchesService = matchesService;   
    }

    [HttpGet("details/{matchId}")]
    public async Task<ActionResult<MatchDetailsDto>> GetMatchDetailsAsync(
        [FromRoute] int matchId,
        CancellationToken cancellationToken = default)
    {
        var response = await _matchesService.GetMatchDetailsAsync(matchId, cancellationToken);
        
        return Ok(response);
    }
}