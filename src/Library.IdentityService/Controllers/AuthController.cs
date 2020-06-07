using System.Threading.Tasks;
using IdentityServer4.Services;
using Library.IdentityService.Models;
using Library.IdentityService.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.IdentityService.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager, IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, true, false);
                if (result.Succeeded)
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Failed to log in");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = new UserAccount { PhoneNumber = model.PhoneNumber, UserName = model.UserName, DateOfBirth = model.DateOfBirth };

                var result = await _userManager.CreateAsync(account, model.Password);
                
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(account, Constants.UserRoleName);
                    await _signInManager.SignInAsync(account, false);
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var context = await _interactionService.GetLogoutContextAsync(logoutId);

            return Redirect(context.PostLogoutRedirectUri);
        }
    }
}