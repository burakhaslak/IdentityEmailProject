using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectEmailWithIdentity.Context;
using ProjectEmailWithIdentity.Entities;
using System;

namespace ProjectEmailWithIdentity.Controllers
{
    public class InboxController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public InboxController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Inbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var values = await _context.Messages.Include(x => x.Category).Where(y => y.ReceiverMail == user.Email && !y.IsTrash).OrderBy(x => x.IsStatus).ThenByDescending(y => y.SendDate).ToListAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> SendMessage()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.CategoryList = new SelectList(categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            message.SenderMail = user.Email;
            message.SendDate = DateTime.Now;
            message.IsStatus = false;
            message.IsTrash = false;
            message.IsStarred = false;
            message.IsDrafted = false;
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("Inbox");
        }

        [HttpGet]
        public async Task<IActionResult> Trash()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var values = await _context.Messages .Include(x => x.Category).Where(y => y.ReceiverMail == user.Email && y.IsTrash) .OrderByDescending(x => x.SendDate).ToListAsync();
            ViewBag.TrashCount = values.Count;
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> RestoreFromTrash(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return Json(new { success = false });

            message.IsTrash = false;
            _context.Update(message);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        public async Task<IActionResult> MoveToTrash(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return Json(new { success = false });

            message.IsTrash = true; 
            _context.Update(message);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        public async Task<IActionResult> MessageDetail(int id)
        {
            var message = await _context.Messages.Include(x => x.Category).FirstOrDefaultAsync(x => x.MessageId == id);

            if (message == null)
            {
                return NotFound();
            }

            var senderUser = await _userManager.FindByEmailAsync(message.SenderMail);
            ViewBag.SenderName = senderUser != null ? $"{senderUser.Name} {senderUser.Surname}" : "Anonymous Sender";
            ViewBag.SenderMail = message.SenderMail;
            ViewBag.Subject = message.Subject;
            ViewBag.MessageDetail = message.MessageDetail;
            ViewBag.SenderImage = senderUser?.ImageUrl ?? "wwwroot/userimages/img2.png";
            ViewBag.SendDate = message.SendDate.ToString("dd MMM yyyy");

            if (!message.IsStatus)
            {
                message.IsStatus = true;
                _context.Update(message);
                await _context.SaveChangesAsync();
            }

            return View(message);
        }

        public async Task<IActionResult> SentMessages()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userMail = user.Email;

            var values = await _context.Messages.Include(x => x.Category).Where(y => y.SenderMail == userMail && !y.IsTrash && !y.IsDrafted).OrderByDescending(x => x.SendDate).ToListAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> Starred()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var values = await _context.Messages.Include(x => x.Category).Where(y => y.ReceiverMail == user.Email && y.IsStarred && !y.IsTrash).OrderByDescending(x => x.SendDate).ToListAsync();
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStarred(int id, string action)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
                return Json(new { success = false });

            switch (action)
            {
                case "star":
                    message.IsStarred = !message.IsStarred;
                    break;
                case "read":
                    message.IsStatus = true;
                    break;
                case "draft":
                    message.IsDrafted = true;
                    break;
            }

            _context.Update(message);
            await _context.SaveChangesAsync();
            return Json(new { success = true, isStarred = message.IsStarred });
        }

        [HttpGet]
        public async Task<IActionResult> Draft()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var values = await _context.Messages.Include(x => x.Category).Where(y => y.SenderMail == user.Email && y.IsDrafted && !y.IsTrash).OrderByDescending(x => x.SendDate).ToListAsync();
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDraft(Message message)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            message.SenderMail = user.Email;
            message.SendDate = DateTime.Now;
            message.IsDrafted = true; 
            message.IsStatus = false;
            message.IsTrash = false;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
