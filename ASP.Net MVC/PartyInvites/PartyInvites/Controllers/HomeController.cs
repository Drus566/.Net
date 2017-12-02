using Microsoft.AspNetCore.Mvc;
using PartyInvites.Models;
using System;
using System.Linq;

namespace PartyInvites.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 12 ? "Good Morning" : "Good Afternoon";
            ViewBag.SomeBag = "228";
            return View("MyView");
        }

        [HttpGet]
        public ViewResult RsvpForm()
        {
            return View();
        }

        [HttpPost]
        public ViewResult RsvpForm(GuestResponse guestResponse)
        {
            if (ModelState.IsValid)
            {
                Repository.AddResponse(guestResponse);
                //Открыть представление Thanks и передать туда параметр guestResponse
                return View("Thanks", guestResponse);
            }
            else
            {
                // тут ошибка в результате проверки
                return View();
            }  
        }

        public ViewResult ListResponses()
        {
            //Так как имя представления не передано, то будет вызываться
            //представление по имени метода public ViewResult ListResponses()
            //Искать система будет в папках
            //Views/Home и Views/Shared
            return View(Repository.Responses.Where(r => r.WillAttend == true));
        }
    }
}