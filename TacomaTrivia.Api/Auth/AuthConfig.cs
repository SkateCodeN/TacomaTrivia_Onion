namespace TacomaTrivia.Api.Auth;


public static class AuthConfig
{
    //policy names
    public const string AdminPolicy = "AdminOnly";
    public const string ReadOnlyPolicy = "ReadOnly";

    //Role Names
    public const string AdminRole = "Admin";
    public const string TestUserRole = "TestUser";
}