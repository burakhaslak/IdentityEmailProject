using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEmailWithIdentity.Context;
using ProjectEmailWithIdentity.Entities;

namespace ProjectEmailWithIdentity.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public DashboardController(UserManager<AppUser> userManager, EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var email = user.Email;

            ViewBag.InboxCount = await _context.Messages.CountAsync(x => x.ReceiverMail == email && !x.IsTrash);
            ViewBag.SentCount = await _context.Messages.CountAsync(x => x.SenderMail == email && !x.IsTrash);
            ViewBag.DraftCount = await _context.Messages.CountAsync(x => x.SenderMail == email && x.IsDrafted);
            ViewBag.StarredCount = await _context.Messages.CountAsync(x => x.ReceiverMail == email && x.IsStarred && !x.IsTrash);

            var lastMessages = await _context.Messages
                .Where(x => x.ReceiverMail == email && !x.IsTrash)
                .OrderByDescending(x => x.SendDate)
                .Take(5)
                .ToListAsync();


            var chartData = new List<int>();
            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.Now.Date.AddDays(-i);
                var count = await _context.Messages
                    .CountAsync(x => x.SenderMail == email && x.SendDate.Date == date && !x.IsTrash);
                chartData.Add(count);
            }
            ViewBag.ChartData = chartData;
      
            return View(lastMessages);
        }
    }
}
