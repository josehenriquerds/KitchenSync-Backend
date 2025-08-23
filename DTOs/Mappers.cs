using KitchenSync.Api.Models;

namespace KitchenSync.Api.DTOs
{
    public static class Mappers
    {
        public static ProductDto ToDto(this Product p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            Category = p.Category,
            PrepSeconds = p.PrepSeconds,
            Available = p.Available,
            Tags = p.Tags,
            IsRecurring = p.IsRecurring,
            Type = p.Type
        };
    }
}
