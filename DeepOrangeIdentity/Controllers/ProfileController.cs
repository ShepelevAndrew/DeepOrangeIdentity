using System.Security.Claims;
using DeepOrangeIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeepOrangeIdentity.Controllers;

[Route("/[action]")]
public class ProfileController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public ProfileController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> ChangeRole(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null) return NotFound();
        
        var userClaims = await _userManager.GetClaimsAsync(user);
    
        var existingClaim = userClaims.FirstOrDefault(c => c.Type == "Employee");

        if (existingClaim == null)
        {
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Employee"));
            await _userManager.UpdateAsync(user);
        }
        else
        {
            return Ok();
        }

        /*if (User.Identity.IsAuthenticated)
        {
            var tokens = await tokenService.RevokeAsync(User);
            var newTokens = await tokenService.CreateTokenAsync(user);
        }*/

        return Ok();
    }
}