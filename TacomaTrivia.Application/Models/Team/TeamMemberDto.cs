using TacomaTrivia.Domain.AggregateRoots.Teams;
public sealed record TeamMemberDto(
    Guid UserId,
    TeamRole Role
);