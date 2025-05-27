using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderTask.Models;

namespace OrderTask.Controllers
{
    public class OrdersController : Controller
    {
        private readonly Context _context;

        public OrdersController(Context context)
        {
            _context = context;
        }

        // GET: Orders with Search and Pagination
        public async Task<IActionResult> Index(string searchString, string searchField, int page = 1)
        {
            var orders = _context.Orders
                .Include(o => o.Governorate)
                .Include(o => o.City)
                .Include(o => o.ProductOrders)
                .ThenInclude(op => op.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                switch (searchField?.ToLower())
                {
                    case "order id":
                        if (int.TryParse(searchString, out int orderId))
                        {
                            orders = orders.Where(o => o.Id == orderId);
                        }
                        break;
                    case "product name":
                        orders = orders.Where(o => o.ProductOrders.Any(op => op.Product.Name.ToLower().Contains(searchString)));
                        break;
                    case "city":
                        orders = orders.Where(o => o.City.Name.ToLower().Contains(searchString));
                        break;
                    case "governorate":
                        orders = orders.Where(o => o.Governorate.Name.ToLower().Contains(searchString));
                        break;
                    case "price":
                        if (decimal.TryParse(searchString, out decimal price))
                        {
                            orders = orders.Where(o => o.ProductOrders.Any(op => op.Product.Price == price));
                        }
                        break;
                    default:
                        orders = orders.Where(o =>
                            o.Name.ToLower().Contains(searchString) ||
                            o.ProductOrders.Any(op => op.Product.Name.ToLower().Contains(searchString)) ||
                            o.City.Name.ToLower().Contains(searchString) ||
                            o.Governorate.Name.ToLower().Contains(searchString));
                        break;
                }
            }

            const int pageSize = 20;
            var totalItems = await orders.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var paginatedOrders = await orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            
            var searchFields = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "All Fields", Selected = string.IsNullOrEmpty(searchField) },
            new SelectListItem { Value = "Order Id", Text = "Order Id", Selected = searchField == "Order Id" },
            new SelectListItem { Value = "Product Name", Text = "Product Name", Selected = searchField == "Product Name" },
            new SelectListItem { Value = "City", Text = "City", Selected = searchField == "City" },
            new SelectListItem { Value = "Governorate", Text = "Governorate", Selected = searchField == "Governorate" },
            new SelectListItem { Value = "Price", Text = "Price", Selected = searchField == "Price" }
        };
            ViewBag.SearchFields = new SelectList(searchFields, "Value", "Text", searchField);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(paginatedOrders);
        }

     
    }
}
