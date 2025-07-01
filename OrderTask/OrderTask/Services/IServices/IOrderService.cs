using OrderTask.Models;
using OrderTask.ViewModel;
using System.Threading.Tasks;

public interface IOrderService
{
    Task<MvcPageList<Order>> GetOrderAsync(string searchString, int pageNumber, int pageSize);
    Task CreateOrderAsync(ProductOrderViewModel viewModel, string createdBy);

}
