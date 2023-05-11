using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet("/login")]
        public IActionResult Login() =>
            SignIn(new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                        new Claim("role_admin","admin")
                    },
                    "cookie",
                    nameType: null,
                    roleType: "role_admin"
                )
            ),
            authenticationScheme: "cookie"
            );

    }
}