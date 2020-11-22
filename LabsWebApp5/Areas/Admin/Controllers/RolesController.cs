using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LabsWebApp5.Areas.Admin.Models;
using LabsWebApp5.Helpers;
using LabsWebApp5.Models;
using static LabsWebApp5.Helpers.Config;

namespace LabsWebApp5.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public RolesController(
            RoleManager<IdentityRole> roleManager, 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

       public IActionResult Index() => View(userManager.Users.OrderBy(u => u.UserName));

       public async Task<IActionResult> Edit(string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = roleManager.Roles.ToList();
                ChangeRoleModel model = new ChangeRoleModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                   // Email = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }
            ModelState.AddModelError(String.Empty,
                "Выбранного пользователя больше не существует");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, List<string> roles)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var addRoles = roles.Except(userRoles);
                var removeRoles = userRoles.Except(roles);
                bool add = addRoles.Any();
                bool remove = removeRoles.Any();

                if (add || remove)
                {
                    var isNotAdmin = await IsNotAdmin();
                    if (!(isNotAdmin is BadRequestResult))
                        return isNotAdmin;
                }

                if (add)
                {
                    if (!user.EmailConfirmed && 
                        (addRoles.Contains(RoleModerator) || addRoles.Contains(RoleWriter)))
                    {
                        user.EmailConfirmed = true;
                        await userManager.UpdateAsync(user);
                    }
                    await userManager.AddToRolesAsync(user, addRoles);
                }
                if (remove) 
                {
                    if(removeRoles.Contains(RoleAdmin))
                    {
                        if(user.UserName == Config.Admin)
                        {
                            ModelState.AddModelError(String.Empty,
                                $"У пользователя \"{Config.Admin}\" роль \"{RoleAdmin}\" зарезервировано сервером, его нельзя удалить. Однако, другие роли можно удалять.");
                            return await Edit(id);
                        }
                        
                        if (User.Identity.Name == user.UserName)
                        {
                            await userManager.RemoveFromRolesAsync(user, removeRoles);
                            await signInManager.RefreshSignInAsync(user);
                            return RedirectToAction(
                                "Info",
                                "Home", 
                                new InfoModel
                                {
                                    Text = $"Вы лишили себя роли {RoleAdmin}"
                                });
                        }
                    }

                    await userManager.RemoveFromRolesAsync(user, removeRoles);
                }

                if (User.Identity.Name == user.UserName && (add || remove)) 
                    await signInManager.RefreshSignInAsync(user);
            }
            else
            {
                ModelState.AddModelError(String.Empty,
                    "Выбранного пользователя больше не существует");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var isNotAdmin = await IsNotAdmin();
                if (!(isNotAdmin is BadRequestResult))
                    return isNotAdmin;

                if (user.UserName == Config.Admin)
                {
                    ModelState.AddModelError(String.Empty,
                        $"Пользователь \"{Config.Admin}\" зарезервирован сервером, его нельзя удалить.");
                    return RedirectToAction("Index");
                }
                await userManager.DeleteAsync(await userManager.FindByIdAsync(id));
            }
            else
            {
                ModelState.AddModelError(String.Empty,
                    "Выбранного пользователя больше не существует");
            }
            return RedirectToAction("Index");
        }

        private async Task<IActionResult> IsNotAdmin()
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                await signInManager.SignOutAsync();
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");

                return RedirectToAction(
                    "Info",
                    "Home",
                    new InfoModel
                    {
                        Text = "Ваш аккаунт был удалён"
                    });
            }

            var isAdmin = await userManager.GetRolesAsync(currentUser);

            if (isAdmin.All(x => x != RoleAdmin))
            {
                await signInManager.RefreshSignInAsync(currentUser);
                return RedirectToAction(
                    "Info",
                    "Home",
                    new InfoModel
                    {
                        Text = $"У Вас нет больше роли {RoleAdmin}, чтобы изменять права пользователям"
                    });
            }

            return BadRequest();
        }
    }
}
