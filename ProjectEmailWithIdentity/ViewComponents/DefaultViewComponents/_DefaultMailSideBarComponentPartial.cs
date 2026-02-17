using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectEmailWithIdentity.Context;
using ProjectEmailWithIdentity.Entities;

namespace ProjectEmailWithIdentity.ViewComponents.DefaultViewComponents
{
    public class _DefaultMailSideBarComponentPartial : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public _DefaultMailSideBarComponentPartial(UserManager<AppUser> userManager, EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.InboxCount = _context.Messages.Where(x => x.ReceiverMail == user.Email && x.IsStatus == false).Count();
            ViewBag.SentCount = _context.Messages.Where(x => x.SenderMail == user.Email && !x.IsDrafted).Count();
            ViewBag.StarCount = _context.Messages.Where(x => x.ReceiverMail == user.Email && x.IsStarred == true).Count();
            ViewBag.TrashCount = _context.Messages.Where(x => x.ReceiverMail == user.Email && x.IsTrash == true).Count();
            ViewBag.DraftCount = _context.Messages.Where(x => x.SenderMail == user.Email && x.IsDrafted == true).Count();
        
            return View();
        }
    }
}
