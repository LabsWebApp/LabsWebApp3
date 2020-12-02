using System.Threading.Tasks;
using LabsWebApp3.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LabsWebApp3.Models.Identity;
using LabsWebApp3.Models;

namespace LabsWebApp3.Controllers
{
    [Authorize]
    public partial class AccountController
    {
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return await EditRegister(returnUrl);
            ViewBag.returnUrl = returnUrl;
            return View(new RegisterModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.UserName
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Config.RoleReader);
                    // установка куки
                    await signInManager.SignInAsync(user, false);

                    return await SendConfirmEmail(user);
                }

                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "InvalidEmail":
                            error.Description = "Не верно указан почтовый адрес";
                            break;
                        case "InvalidUserName":
                            error.Description = "Логин может состоять только из латинских букв и цифр";
                            break;
                        case "DuplicateUserName":
                            error.Description = "Пользователь с таким именем уже существует";
                            break;
                        case "DuplicateEmail":
                            error.Description = "Почтовый адрес привязан к другому пользователю";
                            break;
                        case "InvalidToken":
                            error.Description = "Код устарел, запросите подтверждение повторно";
                            break;
                    }

                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> RegisterEmailConfirm()
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
            {
                await signInManager.SignOutAsync();
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
                return RedirectToAction("Info", "Home",
                    new InfoModel
                    {
                        Title = "Пользователь не найден",
                        Text = "Приносим извинения: возможно Ваш Аккаунт был удалён или заблокирован модератором."
                    });
            }

            return await SendConfirmEmail(user);
        }
    }
}
