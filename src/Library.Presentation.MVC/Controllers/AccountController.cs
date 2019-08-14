using AutoMapper;
using Library.Application.Commands.LoginUser;
using Library.Application.Commands.LogoutUser;
using Library.Application.Commands.RegisterUser;
using Library.Presentation.MVC.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Presentation.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AccountController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = _mapper.Map<RegisterViewModel, RegisterUserCommand>(model);
                var result = _mediator.Send(command).Result;

                if (result.HasErrors)
                {
                    foreach (var error in result.Exceptions.InnerExceptions)
                    {
                        ModelState.AddModelError(string.Empty, error.Message);
                    }
                }
                else
                {
                    var login = new LoginUserCommand
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        RememberMe = false
                    };

                    var res = _mediator.Send(login).Result;

                    return RedirectToAction("Get", "Books");
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
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = _mapper.Map<LoginViewModel, LoginUserCommand>(model);
                var result = _mediator.Send(command).Result;

                if (result.HasErrors)
                {
                    foreach (var error in result.Exceptions.InnerExceptions)
                    {
                        ModelState.AddModelError(string.Empty, error.Message);
                    }
                }
                else
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
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOff()
        {
            var result = _mediator.Send(new LogoutCommand()).Result;
            return RedirectToAction("Get", "Books");
        }
    }
}