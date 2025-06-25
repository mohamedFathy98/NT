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

    public async Task CreateOrderAsync(ProductOrderViewModel viewModel, string createdBy)
    {
        var order = viewModel.Order;
        order.CreatedAt = DateTime.Now;
        order.CreatedBy = createdBy;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        if (viewModel.ProductQuantities != null && viewModel.ProductQuantities.Values.Any(q => q > 0))
        {
            foreach (var pq in viewModel.ProductQuantities)
            {
                int productId = pq.Key;
                int quantity = pq.Value;
                if (quantity > 0)
                {
                    _context.productOrders.Add(new ProductOrder
                    {
                        OrderId = order.Id,
                        ProductId = productId,
                        Quantity = quantity
                    });
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
