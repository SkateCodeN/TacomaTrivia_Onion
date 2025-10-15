namespace TacomaTrivia.Application.Models.User;

/// <summary>Response DTO.</summary>
public sealed record UserDto(
    Guid Id,
    string Name,
    string? Phone,
    string? Email,
    string? Role
);