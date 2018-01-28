using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControllersAndActions.Controllers
{
    public class ExampleController : Controller
    {
        public StatusCodeResult Index()
            => NotFound();
    }
}
