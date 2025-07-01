using OrderTask.Models;
using OrderTask.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class OrderService : IOrderService
{
    private readonly Context _context;

    public OrderService(Context context)
    {
        _context = context;
    }
    public async Task<MvcPageList<Order>> GetOrderAsync(string searchString, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            pageNumber = 1;

        var orders = _context.Orders
                .Include(o => o.City)
                .Include(o => o.Governorate)
                .Include(o => o.ProductOrders) 
                    .ThenInclude(po => po.Product) 
                .AsNoTracking();

        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();
            bool isInt = int.TryParse(searchString, out int idValue);

            orders = orders.Where(o =>
                (isInt && o.Id == idValue) ||
                (o.Name != null && o.Name.ToLower().Contains(searchString)) || 
                (o.Address != null && o.Address.ToLower().Contains(searchString)) ||
                (o.City != null && o.City.Name.ToLower().Contains(searchString)) ||
                (o.Governorate != null && o.Governorate.Name.ToLower().Contains(searchString)) ||
                (o.CreatedBy != null && o.CreatedBy.ToLower().Contains(searchString))

            );
        }

        return await MvcPageList<Order>.CreateAsync(orders, pageNumber, pageSize);
    }


    public async Task CreateOrderAsync(ProductOrderViewModel viewModel, string createdBy)
    {
        var order = viewModel.Order;
        order.CreatedAt = DateTime.UtcNow;
        order.CreatedBy = createdBy;
        _context.Orders.Add(order);

        if (viewModel.ProductQuantities != null && viewModel.ProductQuantities.Values.Any(q => q > 0))
        {
            var productOrders = viewModel.ProductQuantities
                .Where(pq => pq.Value > 0)
                .Select(pq => new ProductOrder
                {
                    Order = order, 
                    ProductId = pq.Key,
                    Quantity = pq.Value
                })
                .ToList();

            _context.productOrders.AddRange(productOrders);
        }

        await _context.SaveChangesAsync();
    }





}

