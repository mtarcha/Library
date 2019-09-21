using System.Threading.Tasks;
using Library.Presentation.MVC.Accounts;
using Library.Presentation.MVC.Clients;
using Library.Presentation.MVC.Models;
using Library.Presentation.MVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Presentation.MVC.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IUsersClient _usersClient;
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;

        public AccountsController(IUsersClient usersClient, UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager)
        {
            _usersClient = usersClient;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
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
                    var user = new AddUserModel { UserName = model.UserName, DateOfBirth = model.DateOfBirth, PhoneNumber = model.PhoneNumber};
                    await _usersClient.AddUser(user);

                    await _signInManager.SignInAsync(account, false);
                    return RedirectToAction("Search", "Books");
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
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Search", "Books");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Failed to log in");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Search", "Books");
        }
    }
}