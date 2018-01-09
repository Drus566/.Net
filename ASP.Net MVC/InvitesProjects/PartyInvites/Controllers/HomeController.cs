using System;
using Microsoft.AspNetCore.Mvc;
using PartyInvites.Models;
using System.Linq;

namespace PartyInvites.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;

        public HomeController(IRepository repo){
            this.repository = repo;
        }

        public ViewResult Index(){
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 12 ? "Good Morning" : "Good Afternoon";
            return View("MyView");
        }

        [HttpGet]
        public ViewResult RsvpForm() => View();

        [HttpPost]
        public ViewResult RsvpForm(GuestReponse guestReponse){
            if(ModelState.IsValid){
                repository.AddResponse(guestReponse);
                return View("Thanks", guestReponse);
            }else{
                //there is a validation error
                return View();
            }
        }

        public ViewResult ListResponses() => 
            View(repository.Reponses.Where(r => r.WillAttend == true));
    }
}