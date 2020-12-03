using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using LabsWebApp3.Areas.Chat.Models;
using Microsoft.AspNetCore.Authorization;
using LabsWebApp3.Models;
using LabsWebApp3.Models.Domain;

using static LabsWebApp3.Helpers.Config;

namespace LabsWebApp3.Areas.Chat.Controllers
{
    [Area("Chat")]
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly DataManager dataManager;

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            DataManager dataManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dataManager = dataManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            var roles = await userManager.GetRolesAsync(user);
            if (user is null || !roles.Contains(RoleReader))
            {
                string name = User.Identity.Name;
                if (user is null)
                {
                    await signInManager.SignOutAsync();
                    HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
                }
                else await signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Info", "Home",
                    new InfoModel
                    {
                        Title = $"{name} в чате не найден",
                        Text = "Приносим извинения: возможно Ваш Аккаунт был удалён или заблокирован модератором."
                    });
            }
            await signInManager.RefreshSignInAsync(user);
            var upto = await dataManager.Functions.GetBlockAsync(user.Id);
            return View(new ChatModel
            {
                UserName = user.UserName,
                UpTo = upto,
                IsModerator = roles.Contains(RoleModerator),
                IsWriter = roles.Contains(RoleWriter),
                IsBlocked = upto >= DateTime.Now,
                Email = user.Email
            });
        }
    }
}
