using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderTask.Models;

namespace OrderTask.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly Context _context;


        public OrdersController(Context context)
        {
            _context = context;

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

            ViewBag.Governorates = new SelectList(_context.governorates, "Id", "Name");
            ViewBag.Cities = new SelectList(_context.Cities, "Id", "Name");
            ViewBag.Products = _context.products.ToList();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, Dictionary<int, int> ProductQuantities)
        {

            if (ModelState.IsValid)
            {
                order.CreatedAt = DateTime.Now;
                order.CreatedBy = User.Identity?.Name;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                if (ProductQuantities != null && ProductQuantities.Values.Any(q => q > 0))
                {
                    foreach (var productQuantity in ProductQuantities)
                    {
                        // Get the quantity for this product, default to 1 if not specified
                        int productId = productQuantity.Key;
                        int quantity = productQuantity.Value;
                        if (quantity > 0)
                            _context.productOrders.Add(new ProductOrder
                            {
                                OrderId = order.Id,
                                ProductId = productId,
                                Quantity = quantity
                            });
                    }
                    await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));



            }
            return View(order);
        }
    }
}
