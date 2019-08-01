using System;
using System.Threading.Tasks;
using Library.Domain;
using Library.Presentation.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Library.Presentation.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly EntityFactory _entityFactory;

        public AccountController(IUnitOfWorkFactory unitOfWorkFactory, EntityFactory entityFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
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
                using (var uow = _unitOfWorkFactory.Create())
                {
                    try
                    {

                        var user = _entityFactory.CreateUser(model.UserName, Role.User);
                        user.SetPassword(model.Password);
                        uow.Users.Create(user);
                        uow.Users.TrySignIn(model.UserName, model.Password, false);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError(string.Empty, e.Message);
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
                using (var uow = _unitOfWorkFactory.Create())
                {
                    if (uow.Users.TrySignIn(model.UserName, model.Password, model.RememberMe))
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Get", "Books");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to log in");
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                uow.Users.SignOut();
                return RedirectToAction("Get", "Books");
            }
        }
    }
}