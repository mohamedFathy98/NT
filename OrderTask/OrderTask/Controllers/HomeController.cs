using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderTask.Controllers.Base;
using OrderTask.Models;
using OrderTask.Services;
using OrderTask.Services.IServices;
using OrderTask.ViewModel;

namespace OrderTask.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly Context _context;
        private readonly IProductService _productService;

        public HomeController( Context context , IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 20;
            // Ensure pageNumber is at least 1
            if (pageNumber < 1) pageNumber = 1;
            var products = await _productService.SearchProductsForHomeAsync(searchString, pageNumber, pageSize);
            return View(products); // products is MvcPageList<Product>
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
