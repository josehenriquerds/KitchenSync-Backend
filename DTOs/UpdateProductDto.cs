using KitchenSync.Api.Models;

namespace KitchenSync.Api.DTOs
{
    public sealed class UpdateProductDto
    {
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public int PrepSeconds { get; set; }
        public bool Available { get; set; }

        public List<string>? Tags { get; set; }
        public bool IsRecurring { get; set; }
        public DishType Type { get; set; }
    }
}
