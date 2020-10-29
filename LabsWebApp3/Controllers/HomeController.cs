using Microsoft.AspNetCore.Mvc;
using LabsWebApp3.Models.Domain;
using System;
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

        public IActionResult Contacts()
        {
            return View("TextOnly", dataManager.TextFields.GetItemByCodeWord("ContactsPage"));
        }

        public IActionResult Privacy()
        {
            return View("TextOnly", dataManager.TextFields.GetItemByCodeWord("PrivacyPage"));
        }
    }
}