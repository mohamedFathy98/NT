using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderTask.Models;
using OrderTask.ViewModel;

namespace OrderTask.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly Context _context;
        private readonly IOrderService _orderService;

        public OrdersController(Context context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        // GET: Orders with Search and Pagination
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, int pageNumber)
        {

            var order = _context.Orders
                .Include(o => o.Governorate)
                .Include(o => o.City)
                .Include(o => o.ProductOrders)
                .ThenInclude(op => op.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                order = order.Where(o =>
                    o.Name.ToLower().Contains(searchString) ||
                    (o.City != null && o.City.Name.ToLower().Contains(searchString)) ||
                    (o.Governorate != null && o.Governorate.Name.ToLower().Contains(searchString)) ||
                    o.CreatedBy.ToLower().Contains(searchString) ||
                    o.ProductOrders.Any(po => po.Product.Price.ToString().Contains(searchString))
                );
            }

            if (pageNumber < 1)
            {
                pageNumber = 1; // Ensure page number is at least 1
            }
            int pageSize = 10; // Adjust page size as needed

            return View(await MvcPageList<Order>.CreateAsync(order, pageNumber, pageSize));

        }

        // GET: Orders/Create

        public IActionResult Create()
        {
            //retrive data from db to dp
            var viewModel = new ProductOrderViewModel
            {
                Products = _context.products.ToList(),
                Governorates = _context.governorates.ToList(),
                Cities = _context.Cities.ToList()
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
