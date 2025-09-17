using Microsoft.AspNetCore.Mvc;
using TacomaTrivia.Application.Models;
using TacomaTrivia.Application.Services;

namespace TacomaTrivia.Api.Controllers;

[ApiController]
[Route("api/venues")]
public sealed class VenuesController(IVenueService svc) : ControllerBase
{
    private readonly IVenueService _svc = svc;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatedVenueRequest req, CancellationToken ct = default)
    {
        var id = await _svc.CreateAsync(req.Name, req.Phone, req.Address, req.Rounds, req.AllowsPets, ct);
        return CreatedAtRoute("GetVenueById", new { id }, null);
    }

    [HttpGet("{id:guid}", Name = "GetVenueById")]
    public async Task<ActionResult<VenueDto>> GetById([FromRoute] Guid id, CancellationToken ct = default)
    {
        var dto = await _svc.GetByIdAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet]
    public Task<IReadOnlyList<VenueDto>> Get([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 25, CancellationToken ct = default)
        => _svc.SearchAsync(q, page, pageSize, ct);

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatedVenue body, CancellationToken ct = default)
    {
        
        await _svc.UpdateAsync(id, body.Name, body.Phone, body.Address, body.AllowsPets, body.Rounds, ct);
        return NoContent();
        
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct = default)
    {
        await _svc.DeleteAsync(id, ct);
        return NoContent();
    }
}
