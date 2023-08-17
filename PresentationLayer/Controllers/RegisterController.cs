using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PresentationLayer.Models;
using MailKit.Net.Smtp;

namespace PresentationLayer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            Random rnd = new Random();
            int x = rnd.Next(100000, 1000000);

            AppUser appUser = new AppUser
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Username,
                ConfirmCode = x
            };

            if (ModelState.IsValid)
            {
                if (model.ConfirmPassword == model.Password)
                {

                    MimeMessage mimeMessage = new MimeMessage();
                    MailboxAddress mailboxAddress = new MailboxAddress("CasgemNewsBit", "gokayacikgoz1@gmail.com");
                    mimeMessage.From.Add(mailboxAddress);

                    MailboxAddress mailboxAddressTo = new MailboxAddress("User", model.Email);
                    mimeMessage.To.Add(mailboxAddressTo);

                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.TextBody = "Giriş yapabilmek için onaylama kodunuz: " + x;
                    mimeMessage.Body = bodyBuilder.ToMessageBody();

                    mimeMessage.Subject = "Doğrulama kodu";

                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Connect("smtp.gmail.com", 587, false);
                    smtpClient.Authenticate("gokayacikgoz1@gmail.com", "ntaqohvgomyaebji");
                    smtpClient.Send(mimeMessage);
                    smtpClient.Disconnect(true);


                    var result = await _userManager.CreateAsync(appUser, model.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "ConfirmedMail", new { id = appUser.Id });
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Şifreler Eşleşmiyor");
                }
            }
            
            return View();
        }
    }
}