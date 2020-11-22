using Microsoft.AspNetCore.Mvc;
using LabsWebApp5.Models.Domain;
using System;
using LabsWebApp5.Models;
using LabsWebApp5.Models.Domain.Entities;

namespace LabsWebApp5.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataManager dataManager;

        public HomeController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public IActionResult Index()
        {
            return View(dataManager.TextFields.GetItemByCodeWord("HomePage"));
        }

        public IActionResult Info(InfoModel model)
        {
            return View(model);
        }

        public IActionResult Contacts()
        {
            return View("TextOnly", dataManager.TextFields.GetItemByCodeWord("ContactsPage"));
        }
    }
}