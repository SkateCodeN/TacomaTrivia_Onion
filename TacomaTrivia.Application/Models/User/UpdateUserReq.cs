namespace TacomaTrivia.Application.Models.User;

using System.ComponentModel.DataAnnotations;
/// <summary>Request DTO for POST.</summary>
public sealed class UpdateUserReq
{
    [Required, MinLength(1)]
    public required string Name { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Role { get; init; }
}
