using Microsoft.AspNetCore.Mvc;
using LabsWebApp3.Models.Domain;

namespace XandCo.Controllers
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
    }
}