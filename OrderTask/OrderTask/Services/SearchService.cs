//using Microsoft.EntityFrameworkCore;
//using OrderTask.Models;
//using OrderTask.Services.IServices;

//namespace OrderTask.Services
//{
    
//    public class SearchService : ISearchService

//    {
//    private readonly Context _context;

//        public SearchService(Context context)
//        {
//            _context = context;
//        }
//        public Task<List<T>> SearchAsync<T>(IQueryable<T> query, string searchString, string searchField, int page = 1, int pageSize = 10) where T : class
//        {
//            if (!string.IsNullOrEmpty(searchString))
//            {
//                searchString = searchString.ToLower();
//                switch (searchField?.ToLower())
//                {
//                    case "id":
//                    case "order id":
//                        if (int.TryParse(searchString, out int id))
//                        {
//                            query = query.Where(e => EF.Property<int>(e, "Id") == id);
//                        }
//                        break;
//                    case "name":
//                    case "product name":
//                        query = query.Where(e => EF.Property<string>(e, "Name").ToLower().Contains(searchString));
//                        break;
//                    case "price":
//                        if (decimal.TryParse(searchString, out decimal price))
//                        {
//                            query = query.Where(e => EF.Property<decimal>(e, "Price") == price);
//                        }
//                        break;
//                    case "Description":
//                    case "governorate":
//                    case "city":
//                        query = query.Where(e => EF.Property<string>(e, "Description").ToLower().Contains(searchString));
//                        break;
//                    default:
//                        query = query.Where(e =>
//                            EF.Property<string>(e, "Name").ToLower().Contains(searchString) ||
//                            //EF.Property<string>(e, "Description").ToLower().Contains(searchString) ||
//                            EF.Property<string>(e, "Governorate").ToLower().Contains(searchString) ||
//                            EF.Property<string>(e, "City").ToLower().Contains(searchString) ||
//                            EF.Property<int>(e, "Id").ToString().Contains(searchString));

//                        break;
//                }
//            }
//            return query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
//        }
//        public Task<int> GetTotalCountAsync<T>(IQueryable<T> query, string searchString, string searchField) where T : class
//        {
//            if (!string.IsNullOrEmpty(searchString))
//            {
//                searchString = searchString.ToLower();
//                switch (searchField?.ToLower())
//                {
//                    case "id":
//                    case "order id":
//                        if (int.TryParse(searchString, out int id))
//                        {
//                            query = query.Where(e => EF.Property<int>(e, "Id") == id);
//                        }
//                        break;
//                    case "name":
//                    case "product name":
//                        query = query.Where(e => EF.Property<string>(e, "Name").ToLower().Contains(searchString));
//                        break;
//                    case "price":
//                        if (decimal.TryParse(searchString, out decimal price))
//                        {
//                            query = query.Where(e => EF.Property<decimal>(e, "Price") == price);
//                        }
//                        break;
//                    case "Description":
//                    case "governorate":
//                    case "city":
//                        query = query.Where(e => EF.Property<string>(e, searchField).ToLower().Contains(searchString));
//                        break;
//                    default:
//                        query = query.Where(e =>

//                            EF.Property<string>(e, "Name").ToLower().Contains(searchString) ||
//                            EF.Property<string>(e, "Description").ToLower().Contains(searchString) ||
//                            EF.Property<string>(e, "Governorate").ToLower().Contains(searchString) ||
//                            EF.Property<string>(e, "City").ToLower().Contains(searchString) ||
//                            EF.Property<int>(e, "Id").ToString().Contains(searchString));
//                        break;
//                }
//            }
//            return query.CountAsync();
//        }




//    }
//}
