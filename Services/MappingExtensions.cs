using KitchenSync.Api.DTOs;
using KitchenSync.Api.Models;

namespace KitchenSync.Api.Services
{
    public static class MappingExtensions
    {
        public static ProductDto ToDto(this Product p)
    => new ProductDto
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


        public static OrderDto ToDto(this Order o)
            => new(
                o.Id,
                o.CreatedAt,
                o.Status,
                o.IsUrgent,
                o.Items.Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.Quantity, i.PrepSeconds)).ToList()
            );
    }
}
