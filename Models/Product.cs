namespace KitchenSync.Api.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int PrepSeconds { get; set; } = 300;
        public bool Available { get; set; } = true;
    }
}
