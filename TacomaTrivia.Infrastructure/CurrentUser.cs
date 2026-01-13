using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TacomaTrivia.Application.Abstractions;

public sealed class CurrentUser:ICurrentUser
{
    //private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly Guid TestUserId = Guid.Parse("12345678-1234-1234-1234-123456789012");
    // public CurrentUser(IHttpContextAccessor httpContextAccessor)
    // {
    //     _httpContextAccessor = httpContextAccessor;
    // }

    public CurrentUser()
    {
        
    }
    //private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    //public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    public bool IsAuthenticated  => true;
    // public Guid UserId
    // {
    //     get
    //     {
    //         var id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //         return Guid.TryParse(id, out var guid) ? guid : Guid.Empty;
    //     }
    // }

    public Guid UserId => TestUserId;


    // public bool IsInRole(string role) => User?.IsInRole(role) == true;
    public bool IsInRole(string role) => true;
}