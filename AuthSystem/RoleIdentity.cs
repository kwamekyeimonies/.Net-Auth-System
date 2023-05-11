using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    o =>
    {
        o.User.RequireUniqueEmail = false;
        o.Password.RequireDigit = false;
        o.Password.RequireLowercase = false;
        o.Password.RequireUppercase = false;
        o.Password.RequireNonAlphanumeric = false;
    }
)
.AddEntityFrameworkStroes<IdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllers();

var app = builder.Build();

using(var scope = app.Services.CreateScope()){
    var usrManager = scope.ServiceProvider.GetRequiredService<UserManger<IdentityUser>>();
    var user = new IdentityUser() {
        UserName = "tenkorangd5@gmail.com",
        Email = "tenkorangd5@gmail.com"
    };

    await UserManager.CreateAsync(user,password:"password");
    await UserManager.AddToRoleAsync(user,"admin");
}

app.UseAuthorization();
app.MapControllers();

app.Run();