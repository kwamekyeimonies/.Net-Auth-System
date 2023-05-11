var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(
    op=>{
        op.DefaultAuthenticateScheme = "cookie";
        op.DefaultSignInScheme = "cookie";
        op.DefaultChallengeScheme = "cookie";
    }
)
.AddCookie("cookie");

builder.Services.AddAuthorization();
 
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();