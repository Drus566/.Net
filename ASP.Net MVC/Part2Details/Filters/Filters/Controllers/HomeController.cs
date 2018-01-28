using Microsoft.AspNetCore.Mvc;
using Filters.Infrastructure;
using System;

namespace Filters.Controllers
{
    [Message("This is the Controller-scoped Filter", Order = 10)]
    public class HomeController : Controller
    {
        [Message("This is the First ACtion-Scoped Filter", Order = 1)]
        [Message("This is the Second ACtion-Scoped Filter", Order = -1)]
        public ViewResult Index() => View("Message",
            "This is the Index action on the Home controller");
    }
}
