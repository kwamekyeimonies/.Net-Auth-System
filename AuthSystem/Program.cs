using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

const string AuthScheme = "cookie";
const string AuthScheme2 = "cookie2";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(AuthScheme)
.AddCookie(AuthScheme)
.AddCookie(AuthScheme2)
;

//Configuration Layer for building Policies
builder.Services.AddAuthorization(
    (builder) =>
    {
        builder.AddPolicy("ecowas passport",
        (pb) =>
        {
            pb.RequireAuthenticatedUser()
            .AddAuthenticationSchemes(AuthScheme)
            .AddRequirements()
            .RequireClaim("passport_type", "ecowas");
        });
    }
);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// app.Use((ctx, next) =>
// {

//     if (ctx.Request.Path.StartsWithSegments("/login"))
//     {
//         return next();
//     }

//     if (!ctx.User.Identities.Any(x => x.AuthenticationType == AuthScheme))
//     {
//         ctx.Response.StatusCode = 401;

//         return Task.CompletedTask;
//     }

//     if (!ctx.User.HasClaim("passport_type", "ecowas"))
//     {
//         ctx.Response.StatusCode = 403;

//         return Task.CompletedTask;
//     }

//     return next();
// });

app.MapGet(
    "/unsecure",
    (HttpContext ctx) =>
    {
        return ctx.User.FindFirst("usr")?.Value ?? "empty";
    }
);


// Using controllers you can specify your policy by adding
// [Authorize(Policy="ecowas passport")]
app.MapGet(
    "/ghana",
    (HttpContext ctx) =>
    {
        // if (!ctx.User.Identities.Any(x => x.AuthenticationType == AuthScheme))
        // {
        //     ctx.Response.StatusCode = 401;

        //     return "Not Authenticated(401)";
        // }

        // if (!ctx.User.HasClaim("passport_type", "ecowas"))
        // {
        //     ctx.Response.StatusCode = 403;
        //     return "Forbidden Response Status Code(403)";
        // }

        return "Passport allowed && Certified";
    }
).RequireAuthorization("ecowas passport");

// There is redirect built into the authorization middleware

app.MapGet(
    "/nigeria",
    (HttpContext ctx) =>
    {
        // if (!ctx.User.Identities.Any(x => x.AuthenticationType == AuthScheme2))
        // {
        //     ctx.Response.StatusCode = 401;

        //     return "Not Authenticated(401)";
        // }

        // if (!ctx.User.HasClaim("passport_type", "ecowas"))
        // {
        //     ctx.Response.StatusCode = 403;
        //     return "Forbidden Response Status Code(403)";
        // }

        return "Passport allowed && Certified";
    }
).RequireAuthorization("ecowas passport");

app.MapGet(
    "/login",
    async (HttpContext ctx) =>
    {
        var claims = new List<Claim>();
        claims.Add(new Claim("usr", "Daniel Tenkorang"));
        claims.Add(new Claim("passport_type", "ecowas"));
        var identity = new ClaimsIdentity(claims, AuthScheme);
        var user = new ClaimsPrincipal(identity);

        await ctx.SignInAsync(AuthScheme, user);
    }
).AllowAnonymous();

// To allow login to reach unauthenticated users we add AllowAnonymous();

app.Run();


public class MyAuthRequirements : IAuthorizationRequirement { }

public class MyAuthRequirementsHandler : AuthorizationHandler<MyAuthRequirements>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyAuthRequirements requirement)
    {
        // context.Succeed(MyAuthRequirements());

        return Task.CompletedTask;
    }
}