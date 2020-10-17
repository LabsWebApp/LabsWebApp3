using Microsoft.AspNetCore.Mvc;
using LabsWebApp3.Models.Domain;
using System;
using LabsWebApp3.Models.Domain.Entities;

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
        public IActionResult Privacy()
        {
            if (dataManager.TextFields.GetItemByCodeWord("PrivacyPage") == null)
                dataManager.TextFields.SaveItem(new TextField
                {
                    CodeWord = "PrivacyPage",
                    Title = "Соглашения",
                    Text = "<h2 align=\"center\">Соглашения</h2>" +
                    "<h1>Заходя на этот сайт, Вы соглашаетесь со следующим:</h1>" +
                        "<h3>8.  Информация о Ваших банковских картах, включая CVR коды, будут украдены, " +
                            "деньги в последствии будут сняты и пропиты разработчиками данного сайта.</h3>" +
                        "<h3>9.  Пароли к Вашим социальным сетям будут взломаны и отданы младшим родственникам и знакомым разработчиков для развлечений.</h3>" +
                        "<h3>10.  Ещё с семью пунктами, который были сознательно скрыты в этом соглашении.</h3>" +
                    "<h6 align=\"center\">Praemonitus, praemunitus</h6>"
                });
            return View("TextOnly", dataManager.TextFields.GetItemByCodeWord("PrivacyPage"));
        }
    }
}