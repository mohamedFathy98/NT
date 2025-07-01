using OrderTask.Models;
using OrderTask.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTask.Services
{
    public class ProductService : IProductService
    {
        private readonly Context _context;

        public ProductService(Context context)
        {
            _context = context;
        }

        public async Task<MvcPageList<Product>> GetProductsAsync(string searchString, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            var products = _context.products.AsNoTracking();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                bool isInt = int.TryParse(searchString, out int idValue);
                bool isDecimal = decimal.TryParse(searchString, out decimal priceValue);

                products = products.Where(p =>
                p.Name.ToLower().Contains(searchString) ||
                p.Description.ToLower().Contains(searchString) ||
                (isInt && p.Id == idValue) ||
                (isDecimal && p.Price == priceValue)
                );
            }
            return await MvcPageList<Product>.CreateAsync(products, pageNumber, pageSize);
        }


        public async Task<int> GetTotalProductsAsync(string searchString)
        {
            var products = _context.products.AsNoTracking();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                products = products.Where(p => p.Name.ToLower().Contains(searchString) ||
                                               p.Description.ToLower().Contains(searchString));
            }
            return await products.CountAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.products.FindAsync(id);
        }

        public async Task CreateProductAsync(Product product)
        {
            _context.products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.products.ToListAsync();
        }
    }
}
