using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

[ApiController]
[Route("api/auth0")]
public class Auth0Controller : ControllerBase
{
    [HttpGet("login")]
    public async Task Login()
    {
        // Point this to a LOCAL route in this controller, to join our backend
        // redirection to our frontend. 
        // in production we only need to set returnUrl to "/auth/user" if that 
        // route is active as we have react being compiled and served via this 
        // backend application
        var returnUrl = Url.Action("AuthCallback");
        var testRole = "Mod";
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



     [HttpGet("logout")]
    public async Task Logout()
    {
        var logoutRedirect = Url.Action("LogoutCallback");
        await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme,  new AuthenticationProperties
        {
            RedirectUri = logoutRedirect
        });
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

     [HttpGet("callback-logout")]
    public IActionResult LogoutCallback()
    {
        // This is where you hardcode (or pull from config) your React URL
        // .NET's "Redirect" method handles absolute URLs perfectly
        return Redirect("http://localhost:5173/login");
    }

    // Lets get info on the role assigned to our user, 
    // we pass it to our frontend

    [HttpGet("me")]
    public IActionResult Me()
    {
        if(!User.Identity?.IsAuthenticated ?? true)
            return Ok(new {isAuthenticated = false});

        var roles = User.Claims
            .Where(c => c.Type == "https://tacomatrivia.com/roles")
            .Select(c => c.Value)
            .ToList();
        
        return Ok( new
        {
            isAuthenticated = true,
            name = User.Identity?.Name,
            roles
        });
    }

    [HttpGet("create/mod")]
    public async Task CreateMod()
    {
        var returnUrl = Url.Action("AuthCallback");
        var testRole = "Mod";
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .WithParameter("ext-role", testRole)
            .Build();


        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }
}