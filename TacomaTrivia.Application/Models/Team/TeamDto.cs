using TacomaTrivia.Domain.AggregateRoots.Teams;
using TacomaTrivia.Application.Models.Team;
namespace TacomaTrivia.Application.Models.Team;
public sealed record TeamDto
(
    Guid Id,
    string Name,
    DateOnly DateCreated,
    IReadOnlyList<TeamMemberDto> Members
);