using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using TacomaTrivia.Application.Models.TeamRecords;
using TacomaTrivia.Application.Services.TeamRecords;

namespace TacomaTrivia.Api.Controllers;

[ApiController]
[Route("api/teamrecords")]

public sealed class TeamRecordController(ITeamRecordSvc svc): ControllerBase
{
    private readonly ITeamRecordSvc _svc = svc;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatedTeamRecordDTO req, CancellationToken ct = default)
    {
        var id = await _svc.CreateAsync(
            req.VenueId,
            req.TeamId,
            req.RecordDate,
            req.TeamName,
            req.Points,
            req.Placed,
            ct
        );
        return CreatedAtRoute("GetTeamRecordById", new {id}, null);
    }

    [HttpGet("{id:guid}", Name = "GetTeamRecordById")]
    public async Task<ActionResult<TeamRecordDTO>> GetById([FromRoute] Guid id, CancellationToken ct = default)
    {
        var dto = await _svc.GetByIdAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet]
    public Task<IReadOnlyList<TeamRecordDTO>> Get([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int size = 25, CancellationToken ct = default)
        => _svc.SearchAsync(q, page, size, ct);

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatedTeamRecordDTO body, CancellationToken ct = default)
    {
        await _svc.UpdateAsync(
            id,
            body.VenueId,
            body.TeamId,
            body.RecordDate,
            body.TeamName,
            body.Points,
            body.Placed,
            ct
        );
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct = default)
    {
        await _svc.DeleteAsync(id, ct);
        return NoContent();
    }

}