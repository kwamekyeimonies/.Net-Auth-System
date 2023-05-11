using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddCookie("local").AddCookie("visitor");

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet(
    "/",
    ctx => Task.FromResult("Auth System development")
);

app.MapGet(
    "/login-local",
    async (ctx) =>
    {
        var claims = new List<Claim>();
        claims.Add(new Claim("usr", "DanielTenkorang"));
        var identity = new ClaimsIdentity(claims, "cookie");
        var user = new ClaimsPrincipal(identity);

        await ctx.SignInAsync("local", user);
    }
);

app.Run();

public class VisitorAuthHandler : CookieAuthenticationHandler
{
    public VisitorAuthHandler(
        IOptionsMonitor<CookieAuthenticationOptions> options, 
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
        ) : base(options,logger,encoder,clock){
            
        }
}