using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEmailWithIdentity.Context;
using ProjectEmailWithIdentity.Entities;
using ProjectEmailWithIdentity.Models;

namespace ProjectEmailWithIdentity.ViewComponents.DefaultViewComponents
{
    public class _DefaultTopBarComponentPartial : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public _DefaultTopBarComponentPartial(UserManager<AppUser> userManager, EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var email = user.Email;
            ViewBag.Name = user.Name + " " + user.Surname;
            ViewBag.Mail = user.Email;
            ViewBag.Image = user.ImageUrl;

            var model = new TopBarMessageViewModel
            {
                UnreadCount = await _context.Messages.CountAsync(x => x.ReceiverMail == email && !x.IsStatus && !x.IsTrash),
                UnreadMessages = await _context.Messages.Where(x => x.ReceiverMail == email && !x.IsStatus && !x.IsTrash).OrderByDescending(x => x.SendDate).Take(5).ToListAsync()
            };

            return View(model);
        }
    }
}
