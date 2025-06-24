namespace OrderTask.Models
{
    public class City
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int GovernorateId { get; set; } // <-- Add this line

        public ICollection<Order> Orders { get; set; }
    }
}
