using DeepOrangeIdentity;
using DeepOrangeIdentity.EF;
using DeepOrangeIdentity.Infrastructure;
using DeepOrangeIdentity.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:5013")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<AuthDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
{

})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                .AddInMemoryApiResources(Configuration.ApiResources)
                .AddInMemoryIdentityResources(Configuration.IdentityResources)
                .AddInMemoryApiScopes(Configuration.ApiScopes)
                .AddInMemoryClients(Configuration.Clients)
                .AddProfileService<ProfileService>()
                .AddDeveloperSigningCredential();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "DeepOrange.Identity.Cockie";
    config.LoginPath = "/login";
    config.LogoutPath = "/logout";
});

builder.Services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "1010419797501-98t0ehmqaea6jlo5trch8v09k86fbk04.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseCors();

app.UseIdentityServer();

app.UseRouting();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=home}/{action=index}/{id?}"
        );

app.Run();