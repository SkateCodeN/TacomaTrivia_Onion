namespace TacomaTrivia.Application.Abstractions;

public interface ICurrentUser
{
    bool IsAuthenticated{get;}
    Guid UserId {get; }
    bool IsInRole(string role);
}