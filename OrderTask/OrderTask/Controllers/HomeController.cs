using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderTask.Models;

namespace OrderTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly Context _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult City()
        {
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Governorate()
        {
            return View();
        }

        public IActionResult Order() 
        {
            
            return View();
            
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
