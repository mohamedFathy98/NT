using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderTask.Controllers.Base;
using OrderTask.Models;

namespace OrderTask.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly Context _context;

        public HomeController( Context context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
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
