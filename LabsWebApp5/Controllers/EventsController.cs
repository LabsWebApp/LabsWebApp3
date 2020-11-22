using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LabsWebApp5.Models.Domain;

namespace LabsWebApp5.Controllers
{
    public class EventsController : Controller
    {
        private readonly DataManager dataManager;

        public EventsController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public IActionResult Index(Guid id)
        {
            if (id != default)
            {
                return View("Show", dataManager.EventItems.GetItemById(id));
            }

            ViewBag.TextField = dataManager.TextFields.GetItemByCodeWord("EventsPage");
            if (dataManager.EventItems.Items.Any())
            { 
                return View(dataManager.EventItems.Items); 
            }

            return View("TextOnly", dataManager.TextFields.GetItemByCodeWord("EventsPage"));
        }
    }
}