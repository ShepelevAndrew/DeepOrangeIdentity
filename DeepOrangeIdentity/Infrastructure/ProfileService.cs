using System.Security.Claims;
using DeepOrangeIdentity.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace DeepOrangeIdentity.Infrastructure;

public class ProfileService : IProfileService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ProfileService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
        
        await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        
        if(user == null) return;
        
        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoles  = await _userManager.GetRolesAsync(user);

        if (await _userManager.IsInRoleAsync(user, "Admin"))
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }

        await _userManager.AddToRoleAsync(user, "Admin");

        if (userClaims.Any(c => c.Value == "Employee"))
        {
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, "Employee"));
        }
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;

        return Task.CompletedTask;
    }
}