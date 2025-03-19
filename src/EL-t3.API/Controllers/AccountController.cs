

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EL_t3.API.Controllers;

[Route("accounts")]
public class AccountController : Controller
{
    [HttpGet("login")]
    public IActionResult Login(string returnUrl = "/")
    {
        var properties = new AuthenticationProperties { RedirectUri = returnUrl };

        return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userName = User.Identity?.Name;
        var userClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

        var userInfo = new
        {
            Name = userName,
            Claims = userClaims
        };

        return Ok(userInfo);
    }
}