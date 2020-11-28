using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabsWebApp3.Areas.Chat.Models;
using Microsoft.AspNetCore.Authorization;
using LabsWebApp3.Models;

namespace LabsWebApp3.Areas.Chat.Controllers
{
    [Area("Chat")]
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
            {
                string name = User.Identity.Name;
                await signInManager.SignOutAsync();
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
                return RedirectToAction("Info", "Home",
                    new InfoModel
                    {
                        Title = $"{name} не найден",
                        Text = "Приносим извинения: возможно Ваш Аккаунт был удалён или заблокирован модератором."
                    });
            }

            return View(new ChatModel
            {
                UserName = user.UserName,
                Email = user.Email
            });
        }
    }
}
