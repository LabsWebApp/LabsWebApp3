using System.Threading.Tasks;
using LabsWebApp5.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LabsWebApp5.Models.Identity;

namespace LabsWebApp5.Controllers
{
    [Authorize]
    public partial class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            var roles = await userManager.GetRolesAsync(user);
                            if (roles?.Contains(Config.RoleAdmin)==true)
                                returnUrl = "/Admin";
                            else if (roles?.Contains(Config.RoleReader) == true)
                                returnUrl = "/Chat";
                            else returnUrl = "/";
                        }

                        return Redirect(returnUrl);
                    }
                }
                ModelState.AddModelError(nameof(Models.Identity.LoginModel.UserName), "Неверный логин или пароль");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
            return RedirectToAction("Index", "Home");
        }
    }
}