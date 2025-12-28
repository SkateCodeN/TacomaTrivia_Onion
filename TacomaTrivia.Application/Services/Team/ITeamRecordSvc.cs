using TacomaTrivia.Application.Models.Team;

namespace TacomaTrivia.Application.Services.Team;

public interface ITeamRecordSvc
{
    Task<IReadOnlyList<TeamRecordDTO>>SearchAsync(string? q, int page, int size,CancellationToken ct);

    Task<TeamRecordDTO?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<Guid> CreateAsync(
        Guid venueId,
        Guid teamId,
        DateOnly recordDate,
        string teamName,
        int? points,
        int? placed,
        CancellationToken ct
    );

    Task UpdateAsync(
        Guid id,
        Guid venueId,
        Guid teamId,
        DateOnly recordDate,
        string teamName,
        int? points,
        int? placed,
        CancellationToken ct
    );

    Task DeleteAsync(Guid id, CancellationToken ct);
}