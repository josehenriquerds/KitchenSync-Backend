using KitchenSync.Api.Models;

namespace KitchenSync.Api.DTOs
{
    public record ProductDto(Guid Id, string Name, string Category, int PrepSeconds, bool Available);
    public record CreateProductDto(string Name, string Category, int PrepSeconds, bool Available);
    public record UpdateProductDto(string Name, string Category, int PrepSeconds, bool Available);
}
