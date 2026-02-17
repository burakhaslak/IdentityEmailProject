using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEmailWithIdentity.Context;
using ProjectEmailWithIdentity.DTOs;
using ProjectEmailWithIdentity.Entities;

namespace ProjectEmailWithIdentity.Controllers
{
    [Authorize]
    public class MyProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public MyProfileController(UserManager<AppUser> userManager, EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            AppUserEditDto appUserEditDto = new AppUserEditDto();
            appUserEditDto.Name = values.Name;
            appUserEditDto.Surname = values.Surname;
            appUserEditDto.Username = values.UserName;
            appUserEditDto.PhoneNumber = values.PhoneNo;
            appUserEditDto.Mail = values.Email;
            appUserEditDto.ImageUrl = values.ImageUrl;
            appUserEditDto.Address = values.Address;
            appUserEditDto.LocationUrl = values.LocationUrl;
            appUserEditDto.About = values.About;

            ViewBag.MapLocation = values.LocationUrl;
            ViewBag.messageCount = _context.Messages.Where(x => x.ReceiverMail == values.Email).Count();
            ViewBag.ContactCount = await _context.Messages.Where(x => x.SenderMail == values.Email).Select(x => x.ReceiverMail).Distinct().CountAsync();

            return View(appUserEditDto);
        }


        [HttpPost]
        public async Task<IActionResult> Profile(AppUserEditDto appUserEditDto)
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

          
            user.Name = appUserEditDto.Name;
            user.Surname = appUserEditDto.Surname;
            user.PhoneNo = appUserEditDto.PhoneNumber;
            user.About = appUserEditDto.About;
            user.LocationUrl = appUserEditDto.LocationUrl;

            if (appUserEditDto.ImagePath != null)
            {
                var resource = Directory.GetCurrentDirectory();
                var extension = Path.GetExtension(appUserEditDto.ImagePath.FileName); 
                var imagename = Guid.NewGuid() + extension;
                var savelocation = Path.Combine(resource, "wwwroot", "userimages", imagename); 

                var stream = new FileStream(savelocation, FileMode.Create);
                await appUserEditDto.ImagePath.CopyToAsync(stream);
                await stream.DisposeAsync(); 

                user.ImageUrl = "/userimages/" + imagename; 
            }

            if (!string.IsNullOrEmpty(appUserEditDto.Password))
            {
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, appUserEditDto.CurrentPassword);

                if (!isPasswordCorrect)
                {
                    ModelState.AddModelError("", "To change your password, you must enter your current password correctly.");
                    return View(appUserEditDto);
                }

  
                if (appUserEditDto.Password != appUserEditDto.ConfirmPassword)
                {
                    ModelState.AddModelError("", "The new passwords do not match.");
                    return View(appUserEditDto);
                }

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, appUserEditDto.Password);
            }

      
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(appUserEditDto.Password))
                {
                    return RedirectToAction("UserLogin", "Login");
                }

                return RedirectToAction("Profile", "MyProfile");
            }

            return View(appUserEditDto);
        }
    }

}
