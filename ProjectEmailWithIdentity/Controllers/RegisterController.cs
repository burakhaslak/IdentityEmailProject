using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using ProjectEmailWithIdentity.DTOs;
using ProjectEmailWithIdentity.Entities;


namespace ProjectEmailWithIdentity.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public RegisterController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRegisterDto createUserRegisterDto)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                int code;
                code = random.Next(100000, 1000000);
                AppUser appuser = new AppUser()
                {
                    Name = createUserRegisterDto.Name,
                    Email = createUserRegisterDto.EMail,
                    Surname = createUserRegisterDto.Surname,
                    UserName = createUserRegisterDto.UserName,
                    EmailConfirmed = false,
                    ConfirmCode = code

                };

                var result = await _userManager.CreateAsync(appuser, createUserRegisterDto.Password);
                if (result.Succeeded)
                {
                    MimeMessage mimeMessage = new MimeMessage();
                    MailboxAddress mailboxAddressFrom = new MailboxAddress("Brock Mail Admin", _configuration["EmailSettings:Username"]);
                    MailboxAddress mailboxAddressTo = new MailboxAddress("User", appuser.Email);

                    mimeMessage.From.Add(mailboxAddressFrom);
                    mimeMessage.To.Add(mailboxAddressTo);


                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.TextBody = "Verification Code to Sign Up in Our Mail Platform:" + code;

                    mimeMessage.Body = bodyBuilder.ToMessageBody();

                    mimeMessage.Subject = "Brock Mail Verification";

                    SmtpClient client = new SmtpClient();
                    client.Connect(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), false);
                    client.Authenticate(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                    client.Send(mimeMessage);
                    client.Disconnect(true);


                    TempData["Mail"] = createUserRegisterDto.EMail;
                    return RedirectToAction("Index", "ConfirmMail");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
           
            return View();



        }
    }
}




