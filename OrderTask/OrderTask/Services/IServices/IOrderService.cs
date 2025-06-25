using OrderTask.Models;
using OrderTask.ViewModel;
using System.Threading.Tasks;

public interface IOrderService
{
    Task CreateOrderAsync(ProductOrderViewModel viewModel, string createdBy);
}
