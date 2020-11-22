using System.Threading.Tasks;
using LabsWebApp5.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LabsWebApp5.Models.Identity;
using LabsWebApp5.Models;

namespace LabsWebApp5.Controllers
{
    [Authorize]
    public partial class AccountController
    {
        public async Task<IActionResult> EditRegister(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
                return await Register(returnUrl);

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

            ViewBag.returnUrl = returnUrl;
            return View(new EditRegisterModel
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditRegister(EditRegisterModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return await Register(ViewBag.returnUrl ?? "/");

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

            if (ModelState.IsValid)
            {
                bool todoEmail = false, todoName = false;
                if (user.NormalizedEmail != model.Email.ToUpper())
                {
                    todoEmail = true;
                    user.Email = model.Email;
                    user.EmailConfirmed = false;
                }

                if (user.UserName != model.UserName)
                {
                    if (Config.Admin.ToUpper() == user.UserName.ToUpper())
                    {
                        ModelState.AddModelError(string.Empty,
                            $"Имя \"{Config.Admin}\" зарезервировано сервером, его нельзя изменять. Однако, пароль рекомендуется поменять, а email можно изменить.");
                        return View(model);
                    }
                    todoName = true;
                    user.UserName = model.UserName;
                }

                if (todoName || todoEmail)
                {
                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        if (todoName)
                            await signInManager.SignInAsync(user, false);
                        if (todoEmail && !user.EmailConfirmed)
                            return await SendConfirmEmail(user);
                        return Redirect(ViewBag.returnUrl ?? "/");
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
                                model.Email = "";
                                break;
                        }
                        ModelState.AddModelError(string.Empty, error.Description);
                        return View(model);
                    }
                }

                return Redirect(ViewBag.returnUrl ?? "/");
            }
            return View(model);
        }
    }
}
