using Microsoft.AspNetCore.Mvc;
using NhlBackend.Contracts.Services;
using NhlBackend.Models.DataTransferObjects;
using NhlBackend.Models.Types;

namespace NhlBackend.Controllers;

[Route("teams")]
public class TeamsController : ControllerBase
{
    private readonly ITeamsService _teamsService;

    public TeamsController(ITeamsService teamsService)
    {
        _teamsService = teamsService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IReadOnlyCollection<TeamDto>>> GetAllTeamsAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await _teamsService.GetAllTeamsAsync(cancellationToken);
        
        return Ok(response);
    }
    
    [HttpGet("performance/{teamId}/{season}")]
    public async Task<ActionResult<TeamPerformanceDto>> GetTeamPerformanceAsync(
        [FromRoute] int teamId,
        [FromRoute] YearOnly season,
        CancellationToken cancellationToken = default)
    {
        var response = await _teamsService.GetTeamPerformanceAsync(teamId, season, cancellationToken);
        
        return Ok(response);
    }
}