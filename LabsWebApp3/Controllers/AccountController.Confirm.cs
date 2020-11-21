using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using LabsWebApp3.Controllers.Helpers;
using LabsWebApp3.Helpers;
using LabsWebApp3.Models;
using Microsoft.AspNetCore.Identity;

namespace LabsWebApp3.Controllers
{
    public partial class AccountController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Info", "Home",
                    new InfoModel
                    {
                        Title = "Подтверждение не удалось",
                        Text = "Приносим извинения: сервер подтверждения временно не работает, повторите попытку позже."
                    });
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {

                return RedirectToAction("Info", "Home",
                    new InfoModel
                    {
                        Title = "Подтверждение не удалось",
                        Text = "Приносим извинения: возможно Ваш Аккаунт был удалён или заблокирован модератором."
                    });
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                string text = String.Empty;
                if (await userManager.IsInRoleAsync(user, Config.RoleReader))
                {
                    await userManager.AddToRoleAsync(user, Config.RoleWriter);
                    text = "Теперь Вы можете полноценно общаться в чате.";
                    await signInManager.RefreshSignInAsync(user);
                }
                return RedirectToAction("Info", "Home",
                    new InfoModel
                    {
                        Title = "Email подтверждён",
                        Text = text
                    });
            }

            return RedirectToAction("Info", "Home",
                new InfoModel
                {
                    Title = "Подтверждение не удалось",
                    Text = "Приносим извинения: сервер подтверждения временно не работает, повторите попытку позже."
                });
        }

        [AllowAnonymous]
        public async Task<IActionResult> SendConfirmEmail(IdentityUser user)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, code },
                protocol: HttpContext.Request.Scheme);
            EmailService emailService = new EmailService();
            var admin = await userManager.FindByNameAsync(Config.Admin);
            var res = await emailService.SendEmailAsync(
                user.UserName, 
                user.Email,
                admin.Email,
                "Подтвердите Ваш Email",
                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
            string text = string.IsNullOrEmpty(res)
                ? "Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме. Вам доступен чат, но возможность оставлять сообщения появятся только после подтверждения."
                : $"Возникли трудности: {res}. Повторите попытку позже. Вам доступен чат, но возможность оставлять сообщения появятся только после подтверждения.";
            return RedirectToAction("Info", "Home", new InfoModel
            {
                Text = text
            });
        }
    }
}
