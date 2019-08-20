using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Library.Presentation.MVC.Clients;
using Library.Presentation.MVC.Models;
using Library.Presentation.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Library.Presentation.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersClient _usersClient;
        private readonly IMapper _mapper;

        public AccountController(IUsersClient usersClient, IMapper mapper)
        {
            _usersClient = usersClient;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<RegisterViewModel, RegisterUserModel>(viewModel);
                var result = await _usersClient.Register(model);

                if (result.ResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, result.ResponseMessage.Content.ToString());
                }
                else
                {
                    await _usersClient.Login(new LoginUserModel
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        RememberMe = false
                    });

                    return RedirectToAction("Get", "Books");
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<LoginViewModel, LoginUserModel>(viewModel);
                var result = await _usersClient.Login(model);

                if (result.ResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, result.ResponseMessage.Content.ToString());
                }
                else
                {
                    if (!string.IsNullOrEmpty(viewModel.ReturnUrl) && Url.IsLocalUrl(viewModel.ReturnUrl))
                    {
                        return Redirect(viewModel.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Get", "Books");
                    }
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _usersClient.LogOff();
            return RedirectToAction("Get", "Books");
        }
    }
}