namespace KitchenSync.Api.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public bool IsUrgent { get; set; } = false;
        public List<OrderItem> Items { get; set; } = new();
    }
}
