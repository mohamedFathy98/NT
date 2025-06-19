namespace OrderTask.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        //public ICollection<Order> orders { get; set; } = new List<Order>();
        //public ICollection<ProductOrder>  productOrders { get; set; } = new List<ProductOrder>();
        public ICollection<ProductOrder>? ProductOrders { get; set; }
        //public DateTime CreatedAt { get; set; }

        //public DateTime CreatedDate { get; set; }
        //public DateTime ModifiedDate { get; set; }
    }
}
