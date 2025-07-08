using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderTask.Models;
using OrderTask.Services;
using OrderTask.Services.IServices;
using OrderTask.ViewModel;

namespace OrderTask.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly Context _context;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
       



        public OrdersController(IProductService productService, Context context, IOrderService orderService  )
        {
            _context = context;
            _orderService = orderService;
            _productService = productService;
        }

        // GET: Orders with Search and Pagination
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 10;
            // Ensure pageNumber is at least 1
            if (pageNumber < 1) pageNumber = 1;
            var orders = await _orderService.GetOrderAsync(searchString, pageNumber, pageSize);

            return View(orders); // orders is MvcPageList<Order>
        }

        // GET: Orders/Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductOrderViewModel
            {
                Products = await _productService.GetAllProductsAsync(),
                Governorates = await _context.governorates.ToListAsync(),
                Cities = await _context.Cities.ToListAsync()
            };
            return View(viewModel);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductOrderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _orderService.CreateOrderAsync(viewModel, User.Identity?.Name ?? "Unknown");
                return RedirectToAction(nameof(Index));
            }

            // Repopulate lists if model state is invalid
            viewModel.Products = _context.products.ToList();
            viewModel.Governorates = _context.governorates.ToList();
            viewModel.Cities = _context.Cities.ToList();
            return View(viewModel);
        }
       

    }
}
