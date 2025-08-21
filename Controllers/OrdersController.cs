using KitchenSync.Api.DTOs;
using KitchenSync.Api.Hubs;
using KitchenSync.Api.Models;
using KitchenSync.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KitchenSync.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly InMemoryStore _store;
        private readonly IHubContext<KitchenHub> _hub;
        public OrdersController(InMemoryStore store, IHubContext<KitchenHub> hub) { _store = store; _hub = hub; }

        [HttpGet]
        public ActionResult<IEnumerable<OrderDto>> Get()
            => Ok(_store.GetOrders().Select(o => o.ToDto()));

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto)
        {
            if (dto.Items is null || dto.Items.Count == 0) return BadRequest("No items.");

            var order = new Order { IsUrgent = dto.IsUrgent }; // ⬅️ NOVO

            foreach (var it in dto.Items)
            {
                var p = _store.GetProduct(it.ProductId);
                if (p is null || !p.Available) return BadRequest($"Invalid product {it.ProductId}");
                order.Items.Add(new OrderItem { ProductId = p.Id, ProductName = p.Name, Quantity = it.Quantity, PrepSeconds = p.PrepSeconds });
            }

            _store.AddOrder(order);
            var outDto = order.ToDto();
            await _hub.Clients.All.SendAsync("order:created", outDto);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, outDto);
        }


        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateStatus(Guid id, UpdateOrderStatusDto dto)
        {
            var ok = _store.UpdateOrderStatus(id, dto.Status);
            if (!ok) return NotFound();
            var ord = _store.GetOrder(id)!.ToDto();
            await _hub.Clients.All.SendAsync("order:updated", ord);
            return NoContent();
        }
    }
}
