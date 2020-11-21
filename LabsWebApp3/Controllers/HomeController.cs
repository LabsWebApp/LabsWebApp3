using Microsoft.AspNetCore.Mvc;
using LabsWebApp3.Models.Domain;
using System;
using LabsWebApp3.Models;
using LabsWebApp3.Models.Domain.Entities;

namespace LabsWebApp3.Controllers
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