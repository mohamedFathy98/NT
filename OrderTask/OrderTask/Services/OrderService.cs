//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using OrderTask.Models;
//using OrderTask.Services.IServices;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace OrderTask.Services
//{
//    public class OrderService : IOrderService
//    {
//        private readonly Context _context;
//        private readonly ISearchService _searchService;
       


//        public OrderService(Context context, ISearchService searchService)
//        {
//            _context = context;
//            _searchService = searchService;
           
            
//        }

//        public async Task<List<Order>> GetOrdersAsync(string searchString, string searchField, int page = 1, int pageSize = 1)
//        {
//            var query = _context.Orders
//                .Include(o => o.Governorate)
//                .Include(o => o.City)
//                .Include(o => o.ProductOrders)
//                .ThenInclude(op => op.Product)
//                .Where(o => o.GovernorateId != null && o.CityId != null)
//                .AsQueryable();
//            return await _searchService.SearchAsync(query, searchString, searchField, page, pageSize);
//        }

//        public async Task<int> GetTotalOrdersAsync(string searchString, string searchField)
//        {
//            var query = _context.Orders
//                .Include(o => o.Governorate)
//                .Include(o => o.City)
//                .Include(o => o.ProductOrders)
//                .ThenInclude(op => op.Product)
//                .AsQueryable();

//            return await _searchService.GetTotalCountAsync(query, searchString, searchField);
//        }

//        public async Task CreateOrderAsync(Order order, Dictionary<int, int> productQuantities)
//        {
//            order.CreatedAt = DateTime.Now;
//            order.CreatedBy = "System"; // Replace with User.Identity?.Name if authentication is set up
//            _context.Orders.Add(order);
//            await _context.SaveChangesAsync();

//            if (productQuantities != null && productQuantities.Values.Any(q => q > 0))
//            {
//                foreach (var productQuantity in productQuantities)
//                {
//                    int productId = productQuantity.Key;
//                    int quantity = productQuantity.Value;
//                    if (quantity > 0)
//                    {
//                        _context.productOrders.Add(new ProductOrder
//                        {
//                            OrderId = order.Id,
//                            ProductId = productId,
//                            Quantity = quantity
//                        });
//                    }
//                }
//                await _context.SaveChangesAsync();
//            }
//        }

//        public SelectList GetGovernorates()
//        {
//            return new SelectList(_context.governorates, "Id", "Name");
//        }

//        public SelectList GetCities()
//        {
//            return new SelectList(_context.Cities, "Id", "Name");
//        }

//        public List<Product> GetProducts()
//        {
//            return _context.products.ToList();
//        }

//        public List<City> GetCitiesByGovernorateId(int governorateId)
//        {
//            return _context.Cities.Where(c => c.GovernorateId == governorateId).ToList();
//        }
//    }
//}