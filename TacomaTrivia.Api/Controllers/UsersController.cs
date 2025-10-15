using Microsoft.AspNetCore.Mvc;
using TacomaTrivia.Application.Models.User;
using TacomaTrivia.Application.Services;

namespace TacomaTrivia.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(IUserService svc) : ControllerBase
{
    private readonly IUserService _svc = svc;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserReq req, CancellationToken ct = default)
    {
        var id = await _svc.CreateAsync(
            req.Name,
            req.Phone,
            req.Email,
            req.Role,
            ct
        );
        return CreatedAtRoute("GetUserById", new { id }, null);
    }

    [HttpGet("{id:guid}", Name = "GetUserById")]
    public async Task<ActionResult<UserDto>> GetById([FromRoute] Guid id, CancellationToken ct = default)
    {
        var dto = await _svc.GetByIdAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet]
    public Task<IReadOnlyList<UserDto>> Get([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 25, CancellationToken ct = default)
        => _svc.SearchAsync(q, page, pageSize, ct);

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserReq body, CancellationToken ct = default)
    {
        
        await _svc.UpdateAsync(
            id,
            body.Name,
            body.Phone,
            body.Email,
            body.Role,
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
