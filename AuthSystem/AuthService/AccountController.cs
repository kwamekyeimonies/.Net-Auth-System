using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpGet("role-login")]
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

    [HttpGet("login-identity")]
    public async Task<IActionResult> Login(SignInManager<IdentityUser> signInManager)
    {
        await signInManager.PasswordSignInAsync(
            "tenkorangd5@gmail.com",
            "password",
            false,
            false
        );

        return Ok();
    }
}