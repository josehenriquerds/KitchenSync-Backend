using KitchenSync.Api.Models;

namespace KitchenSync.Api.DTOs
{
    public sealed class CreateProductDto
    {
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public int PrepSeconds { get; set; }
        public bool Available { get; set; } = true;

        public List<string>? Tags { get; set; }          // opcional no create
        public bool IsRecurring { get; set; } = false;
        public DishType Type { get; set; } = DishType.Prato;
    }
}
