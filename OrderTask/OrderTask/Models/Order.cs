    namespace OrderTask.Models
{
    public class Order
    {
        //public int Id { get; set; }
        //public int Quantity { get; set; }
        //public DateTime OrderDate { get; set; }

        ////public ICollection<Product> Products { get; set; }
        ////public ICollection<ProductOrder> productOrders { get; set; } = new List<ProductOrder>();
        //public City City { get; set; }
        //public Governorate Governorate { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public DateTime DateTime { get; set; }
        public int GovernorateId { get; set; }
        public Governorate? Governorate { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }

        public ICollection<ProductOrder>? ProductOrders { get; set; }
    }
}
