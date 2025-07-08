using OrderTask.Models;

namespace OrderTask.Services.IServices
{
    public interface IProductService
    {
        Task<MvcPageList<Product>> GetProductsAsync(string searchString, int pageNumber, int pageSize);
        Task<int> GetTotalProductsAsync(string searchString);
        Task<Product> GetProductByIdAsync(int id);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<List<Product>> GetAllProductsAsync(); //for creating order
        Task<MvcPageList<Product>> SearchProductsForHomeAsync(string searchString, int pageNumber, int pageSize);

    }
}
