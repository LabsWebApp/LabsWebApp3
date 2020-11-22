using Microsoft.AspNetCore.Mvc;
using LabsWebApp5.Models.Domain;
using LabsWebApp5.Models.Domain.Entities;
using LabsWebApp5.Helpers;

namespace LabsWebApp5.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TextFieldsController : Controller
    {
        private readonly DataManager dataManager;
        public TextFieldsController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public IActionResult Edit(string codeWord)
        {
            var entity = dataManager.TextFields.GetItemByCodeWord(codeWord);
            return View(entity);
        }

        [HttpPost]
        public IActionResult Edit(TextField model)
        {
            if (ModelState.IsValid)
            {
                AutoSEO.Set(model);
                dataManager.TextFields.SaveItem(model);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
            return View(model);
        }
    }
}