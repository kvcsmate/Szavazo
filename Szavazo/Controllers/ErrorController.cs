using Microsoft.AspNetCore.Mvc;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Szavazo.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(string Errormessage = "")
        {
            var error = new Error
            {
                ErrorMessage = Errormessage
            };
            return View(error);
        }
    }
}
