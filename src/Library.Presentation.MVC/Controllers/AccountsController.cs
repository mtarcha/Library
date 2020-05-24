using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Presentation.MVC.Controllers
{
    public class AccountsController : Controller
    {
        public AccountsController()
        {
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction("search", "Books");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            //await _signInManager.SignOutAsync();
            return RedirectToAction("Search", "Books");
        }
    }
}