using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/test-auth")]
public class TestAuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public TestAuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet("token")]
    public IActionResult GetToken(string role)
    {
        // 1. Setup the Identity Claims
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "test-user"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role) // This is what the Policy looks for
        };

        // 2. Get the Key from your Secrets
        var jwtKey = _config["Jwt:Key"] ?? throw new Exception("JWT Key missing!");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 3. Create the Token
        var token = new JwtSecurityToken(
            issuer: "tacomatrivia.com",
            audience: "tacomatrivia.com",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return Ok(new 
        { 
            RoleAssigned = role,
            Token = new JwtSecurityTokenHandler().WriteToken(token) 
        });
    }

    // V2 as we integrate user submitting a username, pass to auth internally via
    // the db

    // UserDto to be used internally when 
    // we get data from the db o
    public class User
    {
        public string Email {get; set;}
        public string Password {get; set;}

        public string? Name {get; set;}
        
    }
    [HttpPost]
    public IActionResult AuthUser([FromBody] User user)
    {
        // We get the info from our frontend 
        string emailHolder = user.Email;
        string passHolder = user.Email;

        //fake function to call db, auth user email and pass and get 
        // a role

        string getRole = "TestUser";

        return GetToken(getRole);

    }
}