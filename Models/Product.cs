namespace KitchenSync.Api.Models
{
    public enum DishType { Prato = 0, Porcao = 1 }

    public sealed class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // <-- Guid (não string)
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public int PrepSeconds { get; set; } = 60;
        public bool Available { get; set; } = true;

        public List<string> Tags { get; set; } = new(); // ex.: sinônimos para busca
        public bool IsRecurring { get; set; } = false;  // “recorrente”
        public DishType Type { get; set; } = DishType.Prato; // Prato | Porcao
    }
}
