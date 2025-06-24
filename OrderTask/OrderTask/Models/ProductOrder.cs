using System.ComponentModel.DataAnnotations;

namespace OrderTask.Models
{
    public class ProductOrder
    {
        [Required(ErrorMessage = "Product selection is required.")]
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }

    }
}
