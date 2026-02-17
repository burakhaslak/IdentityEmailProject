using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectEmailWithIdentity.Entities;
using ProjectEmailWithIdentity.Models;

namespace ProjectEmailWithIdentity.Controllers
{
    public class ConfirmMailController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ConfirmMailController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var email = TempData["Mail"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
               
                return RedirectToAction("CreateUser", "Register");
            }

            ViewBag.v = email;
            var model = new ConfirmMailViewModel { Mail = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ConfirmMailViewModel confirmMail)
        {
            if (string.IsNullOrEmpty(confirmMail.Mail))
            {
                return RedirectToAction("CreateUser", "Register");
            }
            var user = await _userManager.FindByEmailAsync(confirmMail.Mail);
            if (user != null && user.ConfirmCode == confirmMail.ConfirmCode)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("UserLogin", "Login");

            }
            ViewBag.v = confirmMail.Mail;
            ModelState.AddModelError("", "Hatalı onay kodu.");
            return View(confirmMail);
        }
    }
}
