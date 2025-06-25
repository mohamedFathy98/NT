using OrderTask.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTask.ViewModel
{
    public class ProductOrderViewModel
    {
        public Order Order { get; set; } = new Order();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Governorate> Governorates { get; set; } = new List<Governorate>();
        public List<City> Cities { get; set; } = new List<City>();
        // For binding product quantities from the form
        [NotMapped]
        public Dictionary<int, int> ProductQuantities { get; set; } = new Dictionary<int, int>();
    }
}
