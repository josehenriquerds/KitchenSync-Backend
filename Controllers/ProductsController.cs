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
    public class ProductsController : ControllerBase
    {
        private readonly InMemoryStore _store;
        private readonly IHubContext<KitchenHub> _hub;
        public ProductsController(InMemoryStore store, IHubContext<KitchenHub> hub) { _store = store; _hub = hub; }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> Get()
            => Ok(_store.GetProducts().Select(p => p.ToDto()));

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto)
        {
            var p = new Product { Name = dto.Name, Category = dto.Category, PrepSeconds = dto.PrepSeconds, Available = dto.Available };
            _store.AddProduct(p);
            var outDto = p.ToDto();
            await _hub.Clients.All.SendAsync("product:created", outDto);
            return CreatedAtAction(nameof(Get), new { id = p.Id }, outDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateProductDto dto)
        {
            var existing = _store.GetProduct(id);
            if (existing is null) return NotFound();
            existing.Name = dto.Name; existing.Category = dto.Category; existing.PrepSeconds = dto.PrepSeconds; existing.Available = dto.Available;
            _store.UpdateProduct(existing);
            await _hub.Clients.All.SendAsync("product:updated", existing.ToDto());
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!_store.DeleteProduct(id)) return NotFound();
            await _hub.Clients.All.SendAsync("product:deleted", id);
            return NoContent();
        }
    }
}
