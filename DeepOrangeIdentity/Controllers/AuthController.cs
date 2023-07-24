using DeepOrangeIdentity.Models;
using DeepOrangeIdentity.ViewModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web;
using System;

namespace DeepOrangeIdentity.Controllers
{
    [Route("/[action]")]
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _intersectionService;

        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IIdentityServerInteractionService intersectionService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _intersectionService = intersectionService;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnurl)
        {
            var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();

            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnurl,
                ExternalProviders = externalProviders
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            returnUrl = "http://localhost:5013" + returnUrl;
            var uri = new Uri(returnUrl);

            var redirectQuery = HttpUtility.ParseQueryString(uri.Query);

            var redirectUri = redirectQuery.Get("redirect_uri");

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);
            
            return Challenge(properties, provider);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            var user = await _userManager.FindByNameAsync(viewModel.UserName);
            if(user is null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, false, false);

            if(result.Succeeded)
            {
                return Redirect(viewModel.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Login Error");

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Register(string returnurl)
        {
            var viewModel = new RegisterViewModel
            {
                ReturnUrl = returnurl
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            var user = new AppUser
            {
                UserName = viewModel.Username,
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(viewModel.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Login ocurred");

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutResult = await _intersectionService.GetLogoutContextAsync(logoutId);

            return Redirect("tg://resolve?domain=DeepOrange_bot"/*logoutResult.PostLogoutRedirectUri*/);
        }
    }
}
