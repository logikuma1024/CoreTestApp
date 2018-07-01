using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreTestApp.Models;
using CoreTestApp.Hubs;

namespace CoreTestApp.Controllers
{
    public class HomeController : Controller
    {
        private GpioHub  _hub;

        public HomeController(GpioHub chatHub)
        {
            _hub = chatHub;
        }

        public IActionResult Index()
        {
            ViewData["ButtonCount"] = _hub.GetCurrentVal();

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
