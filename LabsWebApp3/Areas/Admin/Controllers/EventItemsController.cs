using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LabsWebApp3.Models.Domain;
using LabsWebApp3.Models.Domain.Entities;
using LabsWebApp3.Helpers;

namespace LabsWebApp3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventItemsController : Controller
    {
        private readonly DataManager dataManager;
        public EventItemsController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public IActionResult Edit(Guid id)
        {
            var entity = id == default ? new EventItem() 
                : dataManager.EventItems.GetItemById(id);
            return View(entity);
        }

        [HttpPost]
        public IActionResult Edit(EventItem model, IFormFile titleImageFile)
        {
            if (ModelState.IsValid)
            {
                if (titleImageFile != null)
                {
                    string saveName = titleImageFile.ToSaveFileName(out bool mustSave);

                    if (mustSave && saveName.Equals(string.Empty))
                    {
                        ModelState.AddModelError("SaveImgError", $"В базе картинок существует другой файл с именем: \"{titleImageFile.FileName}\"," +
                            $" переименуйте исходный файл при дальнейшем редактирование  \"{model.Title}\", или нажмите \"Сохранить\" (без сохранения/изменения картинки).");
                        return View(model);
                    }
                    model.TitleImagePath = titleImageFile.FileName;
                    if (mustSave)
                    {
                        using var stream = new FileStream(saveName, FileMode.Create);
                        titleImageFile.CopyTo(stream);
                    }
                }
                AutoSEO.Set(model);
                dataManager.EventItems.SaveItem(model);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
            return View(model);
        }

        public IActionResult Delete(Guid id)
        {
            dataManager.EventItems.DeleteItem(id);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
    }
}