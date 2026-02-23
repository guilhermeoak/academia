using AcademiaJiuJitsu.Infrastructure.Data;
using AcademiaJiuJitsu.Infrastructure.Extensions;
using AcademiaJiuJitsu.Infrastructure.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
}).AddIdentityCookies();

builder.Services.AddAuthorization();
builder.Services.AddMudServices();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = ["Admin", "Secretaria", "Professor"];
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var email = "admin@academia.local";
    if (await userManager.FindByEmailAsync(email) is null)
    {
        var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
        await userManager.CreateAsync(user, "Admin123!");
        await userManager.AddToRoleAsync(user, "Admin");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<AcademiaJiuJitsu.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
