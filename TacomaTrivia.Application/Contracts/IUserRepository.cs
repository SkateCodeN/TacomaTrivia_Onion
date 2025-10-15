using TacomaTrivia.Domain;

namespace TacomaTrivia.Application.Contracts;

/// <summary>Port that Infrastructure implements.</summary>
public interface IUserRepository
{
    Task<TTUser?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<TTUser>> SearchAsync(string? q, int page, int pageSize, CancellationToken ct);
    Task<Guid> AddAsync(TTUser venue, CancellationToken ct);
    Task UpdateAsync(TTUser venue, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}
