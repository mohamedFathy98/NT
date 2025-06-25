using Microsoft.EntityFrameworkCore;

namespace OrderTask.Models
{
    public class MvcPageList<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; private set; }

    public MvcPageList(List<T> items, int count, int pageIndex, int pageSize)
        {
            if (items != null && items.Any())
            {
                Items = items ?? new List<T>();
                PageIndex = pageIndex;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                Items = items;

            }
        }

        public  bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public int PageSize => Items.Count > 0 ? Items.Count : TotalItems > 0 ? TotalItems : 1; // Avoid division by zero
        public int StartIndex => (PageIndex - 1) * PageSize + 1;
        public int EndIndex => Math.Min(PageIndex * PageSize, TotalItems);


        public static async Task<MvcPageList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync(); //total number of items in the data
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new MvcPageList<T>(items, count, pageIndex, pageSize);
        }
    }

}
