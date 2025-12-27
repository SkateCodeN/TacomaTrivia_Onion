using TacomaTrivia.Domain.AggregateRoots;

namespace TacomaTrivia.Application.Contracts.Teams;

public interface ITeamRecordRepository
{
    Task<TeamRecord?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<TeamRecord>> 
        SearchAsync(string? q, int page, int size, CancellationToken ct);
    Task<Guid> AddAsync(TeamRecord record, CancellationToken ct);
    Task UpdateAsync(TeamRecord record, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}