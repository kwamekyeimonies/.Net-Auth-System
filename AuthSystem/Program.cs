using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);
//Adding Data Protection Layer for Encryption and decrypting of Data
builder.Services.AddDataProtection();

var app = builder.Build();


app.MapGet(
    "/username", (HttpContext ctx, IDataProtectionProvider idp) =>
    {
        var protector = idp.CreateProtector("auth-cookie");


        var authCookie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x.StartsWith("auth="));
        var protectedPayload = authCookie?.Split("=").Last();
        if (protectedPayload != null)
        {
            var payload = protector.Unprotect(protectedPayload);
            var parts = payload.Split(":");
            var key = parts[0];
            var value = parts[1];

            return value;
        }
        else
        {
            return "No auth cookie found.";
        }
    }
);

app.MapGet(
    "/login", (HttpContext ctx, IDataProtectionProvider idp) =>
    {
        var protector = idp.CreateProtector("auth-cookie");
        ctx.Response.Headers["set-cookie"] = $"auth={protector.Protect("user:Kwame Monies")}";
        return "OK";
    }
);


app.Run();