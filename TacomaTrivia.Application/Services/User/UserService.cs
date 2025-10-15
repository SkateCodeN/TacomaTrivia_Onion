using TacomaTrivia.Application.Contracts;
using TacomaTrivia.Application.Models.User;
using TacomaTrivia.Domain;

namespace TacomaTrivia.Application.Services.User;

public sealed class UserService(IUserRepository repo) : IUserService
{
    private readonly IUserRepository _repo = repo;

    public async Task<IReadOnlyList<UserDto>> SearchAsync(
        string? q,
        int page,
        int size,
        CancellationToken ct
    )
    {
        var items = await _repo.SearchAsync(q, page, size, ct);
        // the .. is equivalent to doing .ToList()
        return [.. items.Select(
            usr => new UserDto(
                usr.Id,
                usr.Name,
                usr.Phone,
                usr.Email,
                usr.Role
            )
        )];
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty");
        var usr = await _repo.GetByIdAsync(id, ct);
        return usr is null ? null :
            new UserDto(
                usr.Id,
                usr.Name,
                usr.Phone,
                usr.Email,
                usr.Role
            )
        ;
    }

    public Task<Guid> CreateAsync(
        string name,
        string? phone,
        string? email,
        string? role,
        CancellationToken ct
    )
    => _repo.AddAsync(
        TTUser.Create(name, phone, email, role), ct);

    public async Task UpdateAsync(
        Guid id,
        string name,
        string? phone,
        string? email,
        string? role,
        CancellationToken ct
    )
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be Empty");
        var usr = await _repo.GetByIdAsync(id, ct) ??
            throw new KeyNotFoundException($"User {id} not found");
        usr.Update(name, phone, email, role);
        await _repo.UpdateAsync(usr, ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be Empty");
        var deleted = await _repo.DeleteAsync(id, ct);
        if (!deleted) throw new KeyNotFoundException($"User {id} not found");
    }
}