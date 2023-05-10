using Microsoft.AspNetCore.DataProtection;
using AuthSystem.Service;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
//Adding Data Protection Layer for Encryption and decrypting of Data
builder.Services.AddDataProtection();
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.Use((ctx, next) =>
{
    var idp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
    var protector = idp.CreateProtector("auth-cookie");
    var authCookie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x.StartsWith("auth="));
    if (authCookie != null)
    {
        var protectedPayload = authCookie.Split("=").Last();

        var payload = protector.Unprotect(protectedPayload);
        var parts = payload.Split(":");
        var key = parts[0];
        var value = parts[1];

        var claims = new List<Claim>();
        claims.Add(new Claim(key, value));
        var identity = new ClaimsIdentity(claims);
        ctx.User = new ClaimsPrincipal();

    }
    return next();
}
);

app.MapGet(
    "/username", (HttpContext ctx) =>
    {
        // return ctx.User.FindFirst("usr").Value;
        var user = ctx.User;
        if (user != null)
        {
            var claim = user.FindFirst("usr");
            if (claim != null)
            {
                return claim.Value;
            }
        }

        return "No user claim found";
    }
);

app.MapGet(
    "/login", (AuthService auth) =>
    {
        auth.SignIn();
        return "OK";
    }
);


app.Run();