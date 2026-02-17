using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectEmailWithIdentity.DTOs;
using ProjectEmailWithIdentity.Entities;

namespace ProjectEmailWithIdentity.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        private readonly UserManager<AppUser> _userManager;

        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(LoginUserDto loginUserDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginUserDto.UserName, loginUserDto.Password, false, true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(loginUserDto.UserName);
                if (user.EmailConfirmed == true)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    await _signInManager.SignOutAsync();

                    ModelState.AddModelError("", "Please confirm your email address before logging in.");
                    return View();
                }
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account is locked. Please try again later.");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View();
        }
    }
}

