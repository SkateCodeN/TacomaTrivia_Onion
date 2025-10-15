using TacomaTrivia.Application.Models.User;

namespace TacomaTrivia.Application.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserDto>> SearchAsync(string? q, int page, int pageSize, CancellationToken ct);
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Guid> CreateAsync(
        string name,
        string? phone,
        string? email,
        string? role,
        CancellationToken ct
    );
    Task UpdateAsync(
        Guid id,
        string name,
        string? phone,
        string? email,
        string? role,
        CancellationToken ct
    );
    Task DeleteAsync(Guid id, CancellationToken ct);
}
