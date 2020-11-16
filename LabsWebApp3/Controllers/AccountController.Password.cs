using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LabsWebApp3.Models.Identity;
using LabsWebApp3.Controllers.Helpers;
using LabsWebApp3.Models;
using Microsoft.AspNetCore.Identity;

namespace LabsWebApp3.Controllers
{
    public partial class AccountController
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string email)
        {
            if (!string.IsNullOrEmpty(email))
                ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("Info", "Home", new InfoModel
                    {
                        Title = "Email не найден",
                        Text = $"\"{model.Email}\" не найден в базе данных. Проверьте корректность или пройдите регистрацию."
                    });
                }
                return PasswordEmailConfirm(user, model.Email).Result;
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code)
        {
            if (code is null)
               return RedirectToAction("Info", "Home", new InfoModel
                    {
                        Text = "Приносим извинения: повторите попытку."
                    });
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ForgotPassword");
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Info", "Home", new InfoModel
                {
                    Title = "Смена пароля",
                    Text = "Пароль был успешно изменён!"
                });
            }
            foreach (var error in result.Errors)
            {
                switch (error.Code)
                {
                    case "InvalidToken":
                        return PasswordEmailConfirm(user, model.Email, true).Result;
                }
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        async Task<IActionResult> PasswordEmailConfirm(IdentityUser user, string email, bool obsolete = false)
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { UserId = user.Id, code }, protocol: HttpContext.Request.Scheme);
            EmailService emailService = new EmailService();
            var res = await emailService.SendEmailAsync(user.UserName, email, "Смена пароля",
                $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");

            if (string.Empty == res)
                return RedirectToAction("Info", "Home", new InfoModel
                {
                    Title = obsolete ? "Код устарел": "Проверьте почту",
                    Text = "Для сброса пароля перейдите по ссылке в письме, отправленном на Ваш email."
                });
            else 
                return RedirectToAction("Info", "Home", new InfoModel
                {
                    Text = $"Возникли трудности: {res} Повторите попытку позже."
                });
        }
    }
}
