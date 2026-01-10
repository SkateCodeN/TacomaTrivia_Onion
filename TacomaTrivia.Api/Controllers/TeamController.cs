using Microsoft.AspNetCore.Mvc;
using TacomaTrivia.Application.Models.Team;
using TacomaTrivia.Application.Models.TeamRecords;
using TacomaTrivia.Application.Services.TeamRecords;
using TacomaTrivia.Application.Services.Teams;
using TacomaTrivia.Domain.AggregateRoots.Teams;
namespace TacomaTrivia.Api.Controllers;

[ApiController]
[Route("api/team")]

public sealed class TeamController(ITeamService svc): ControllerBase
{
    private readonly ITeamService _svc = svc;

    // The UI is able to create a new team. 
    [HttpPost]
    public async Task<IActionResult> CreateTeam([FromBody] CreatedTeamDto req, CancellationToken ct = default)
    {
        var id = await _svc.CreateTeamAsync(
            req.TeamName,
            req.TeamOwnerId,
            ct
        );

        return CreatedAtRoute("GetTeamById", new {id}, null);
    }

    [HttpGet("{id: guid}", Name = "GetTeamById")]
    public async Task<ActionResult<TeamDto>> GetById([FromRoute] Guid id, CancellationToken ct = default)
    {
        var dto = await _svc.GetByIdAsync(id, ct);
        return dto is null ? NotFound () : Ok(dto);
    }

    // We are able to search teams when we search in the searchbar
    [HttpGet]
    public async Task<IReadOnlyList<TeamDto>> Get([FromQuery] string? q, [FromQuery] int page =1, [FromQuery] int size = 25, CancellationToken ct = default)
        => await _svc.SearchTeamAsync(q, page, size, ct);

    // Our Frontend user is able to get teams associated with user
    // Note: only authenticated users can create teams.
    // Only team owners can edit them, add team members, update the team, and delete it
    [HttpGet("getTeamsForUser/{userId:guid}")]
    public async Task<IReadOnlyList<TeamDto>> GetTeamsForUserId([FromRoute] Guid userId, CancellationToken ct = default)
        => await _svc.GetTeamsForUserAsync(userId, ct);
  
    //
    [HttpPost("addMember")]
    //  We are setting the enum Owner = 1, so do we pass Owner or 1?
    //  We can pass both
    //  Our owner/admin can add member to the team
    public async Task AddMemberToTeam([FromBody] AddTeamMemberRequest req, CancellationToken ct = default)
    {
        var role = req.Role switch
        {
            TeamRoleDto.Owner => TeamRole.Owner,
            TeamRoleDto.Admin => TeamRole.Admin,
            TeamRoleDto.Member => TeamRole.Member,
            _ => throw new BadHttpRequestException("Bad role cuz")
        };
           
        await _svc.AddMemberAsync(req.TeamId,req.UserId, role, ct);
    }

    [HttpPost("removeMember")]
    //  Owner/admin is able to remove a member from the team
    public async Task RemoveMemberFromTeam([FromBody] RemoveMemberReqDto req, CancellationToken ct = default)
        => await _svc.RemoveMemberAsync(req.TeamId, req.UserId, ct);

    [HttpPost("changeMemberRole")]
    public async Task ChangeMemberRole([FromBody] ChangeMemberRoleReqDto req, CancellationToken ct = default)
    {
        var role = req.NewRole switch
        {
            TeamRoleDto.Owner => TeamRole.Owner,
            TeamRoleDto.Admin => TeamRole.Admin,
            TeamRoleDto.Member => TeamRole.Member,
            _ => throw new BadHttpRequestException("Bad role cuz")
        };

        await _svc.ChangeMemberRoleAsync(req.TeamId, req.UserId, role, ct);
    }

    [HttpDelete("{teamId:guid}")]
    public async Task DeleteTeam([FromRoute] Guid teamId, CancellationToken ct = default)
        => await _svc.DeleteTeamAsync(teamId, ct);
}