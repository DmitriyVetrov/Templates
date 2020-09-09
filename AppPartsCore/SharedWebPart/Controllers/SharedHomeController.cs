using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SharedWebPart
{
    public class SharedHomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new HomeViewModel());
        }
    }
}
