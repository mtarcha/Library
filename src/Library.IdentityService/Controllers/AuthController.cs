using System;
using System.Threading.Tasks;
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

        public AuthController(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
    }
}