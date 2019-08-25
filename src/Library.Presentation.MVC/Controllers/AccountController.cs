using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Library.Presentation.MVC.Clients;
using Library.Presentation.MVC.Models;
using Library.Presentation.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Presentation.MVC.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult Login(string returnUrl = null)
        {
            var u = User;
            return RedirectToAction("Get", "Books");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}