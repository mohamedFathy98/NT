//using Microsoft.EntityFrameworkCore;
//using OrderTask.Models;
//using OrderTask.Services.IServices;
//using System.Threading.Tasks;

//namespace OrderTask.Services
//{
//    public class ProductService : IProductService
//    {
//        private readonly Context _context;
//        private readonly ISearchService _searchService;

//        public ProductService(Context context, ISearchService searchService)
//        {
//            _context = context;
//            _searchService = searchService;
//        }

//        public async Task<List<Product>> GetProductsAsync(string searchString, string searchField, int page = 1, int pageSize = 10)
//        {
//            var query = _context.products.AsQueryable();
//            return await _searchService.SearchAsync(query, searchString, searchField, page, pageSize);
//        }

//        public async Task<int> GetTotalProductsAsync(string searchString, string searchField)
//        {
//            var query = _context.products.AsQueryable();
//            return await _searchService.GetTotalCountAsync(query, searchString, searchField);
//        }
//    }
//}