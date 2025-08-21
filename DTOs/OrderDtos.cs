using KitchenSync.Api.Models;

namespace KitchenSync.Api.DTOs
{
    public record OrderItemDto(Guid ProductId, string ProductName, int Quantity, int PrepSeconds);

    public record OrderDto(Guid Id, DateTime CreatedAt, OrderStatus Status, bool IsUrgent, List<OrderItemDto> Items);

    public record CreateOrderItemDto(Guid ProductId, int Quantity);

    public record CreateOrderDto(List<CreateOrderItemDto> Items, bool IsUrgent = false);

    public record UpdateOrderStatusDto(OrderStatus Status);
}
