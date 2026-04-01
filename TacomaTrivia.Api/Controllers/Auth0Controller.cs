using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;

[ApiController]
[Route("api/auth0")]
public class Auth0Controller : ControllerBase
{
    public async Task Login()
    {
        // Point this to a LOCAL route in this controller, to join our backend
        // redirection to our frontend. 
        // in production we only need to set returnUrl to "/auth/user" if that 
        // route is active as we have react being compiled and served via this 
        // backend application
        var returnUrl = Url.Action("AuthCallback");
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

    [HttpGet("callback-final")]
    public IActionResult AuthCallback()
    {
        // This is where you hardcode (or pull from config) your React URL
        // .NET's "Redirect" method handles absolute URLs perfectly
        return Redirect("http://localhost:5173/auth/user");
    }
}