using Microsoft.AspNetCore.Mvc;
using LabsWebApp5.Models.Domain;

namespace LabsWebApp5.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly DataManager dataManager;

        public HomeController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public IActionResult Index()
        {
            return View(dataManager.EventItems.Items);
        }
    }
}