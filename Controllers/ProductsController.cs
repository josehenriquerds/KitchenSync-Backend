using KitchenSync.Api.DTOs;
using KitchenSync.Api.Hubs;
using KitchenSync.Api.Models;   // DishType, Product
using KitchenSync.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using DtoMap = KitchenSync.Api.DTOs.Mappers; // 👈 alias para ToDto

namespace KitchenSync.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly InMemoryStore _store;
        private readonly IHubContext<KitchenHub> _hub;

        public ProductsController(InMemoryStore store, IHubContext<KitchenHub> hub)
        {
            _store = store;
            _hub = hub;
        }

        // ------------------------------------------------------
        // GET /api/products
        // Filtros (query):
        //   q=texto
        //   cat=Carnes,Saladas            (CSV)
        //   onlyAvailable=true|false      (default: true)
        //   recurring=true|false
        //   portion=true|false            (atalho p/ Type=Porcao)
        //   type=Prato|Porcao
        //   sort=az|fast|sold
        //   take=50
        // ------------------------------------------------------
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> Get(
            [FromQuery] string? q,
            [FromQuery(Name = "cat")] string? categoriesCsv,
            [FromQuery] bool? onlyAvailable,
            [FromQuery] bool? recurring,
            [FromQuery] bool? portion,
            [FromQuery] DishType? type,
            [FromQuery] string? sort,
            [FromQuery] int? take)
        {
            IEnumerable<Product> query = _store.GetProducts();

            // Disponibilidade (default = true)
            if (onlyAvailable is null || onlyAvailable == true)
                query = query.Where(p => p.Available);

            // Categorias (CSV)
            if (!string.IsNullOrWhiteSpace(categoriesCsv))
            {
                var set = new HashSet<string>(
                    categoriesCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                    StringComparer.OrdinalIgnoreCase
                );
                query = query.Where(p => set.Contains(p.Category));
            }

            // Recorrentes
            if (recurring is not null)
                query = query.Where(p => p.IsRecurring == recurring.Value);

            // Porção (atalho)
            if (portion is not null)
                query = query.Where(p => (p.Type == DishType.Porcao) == portion.Value);

            // Tipo explícito
            if (type is not null)
                query = query.Where(p => p.Type == type.Value);

            // Busca em Name/Tags
            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                query = query.Where(p =>
                    p.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (p.Tags != null && p.Tags.Any(t => t.Contains(term, StringComparison.OrdinalIgnoreCase))));
            }

            // Ordenação
            query = (sort ?? "az").ToLowerInvariant() switch
            {
                "fast" => query.OrderBy(p => p.PrepSeconds).ThenBy(p => p.Name),
                "sold" => query.OrderBy(p => p.Name), // placeholder
                _ => query.OrderBy(p => p.Name)
            };

            if (take is > 0) query = query.Take(take.Value);

            return Ok(query.Select(DtoMap.ToDto).ToList());
        }

        // ------------------------------------------------------
        // GET /api/products/{id}
        // ------------------------------------------------------
        [HttpGet("{id:guid}")]
        public ActionResult<ProductDto> GetById(Guid id)
        {
            var p = _store.GetProduct(id);
            return p is null ? NotFound() : Ok(DtoMap.ToDto(p));
        }

        // ------------------------------------------------------
        // GET /api/products/categories
        // -> [{ name, count }]
        // ------------------------------------------------------
        [HttpGet("categories")]
        public ActionResult<IEnumerable<object>> GetCategories()
        {
            var data = _store.GetProducts()
                .GroupBy(p => p.Category)
                .OrderBy(g => g.Key)
                .Select(g => new { name = g.Key, count = g.Count() })
                .ToList();

            return Ok(data);
        }

        // ------------------------------------------------------
        // POST /api/products
        // ------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto)
        {
            var p = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                PrepSeconds = dto.PrepSeconds,
                Available = dto.Available,
                IsRecurring = dto.IsRecurring,
                Type = dto.Type,
                Tags = dto.Tags ?? new List<string>()
            };

            _store.AddProduct(p);
            var outDto = DtoMap.ToDto(p);
            await _hub.Clients.All.SendAsync("product:created", outDto);
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, outDto);
        }

        // ------------------------------------------------------
        // PUT /api/products/{id}
        // ------------------------------------------------------
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateProductDto dto)
        {
            var existing = _store.GetProduct(id);
            if (existing is null) return NotFound();

            existing.Name = dto.Name;
            existing.Category = dto.Category;
            existing.PrepSeconds = dto.PrepSeconds;
            existing.Available = dto.Available;
            existing.IsRecurring = dto.IsRecurring;
            existing.Type = dto.Type;
            existing.Tags = dto.Tags ?? new List<string>();

            _store.UpdateProduct(existing);
            await _hub.Clients.All.SendAsync("product:updated", DtoMap.ToDto(existing));
            return NoContent();
        }

        // ------------------------------------------------------
        // DELETE /api/products/{id}
        // ------------------------------------------------------
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!_store.DeleteProduct(id)) return NotFound();
            await _hub.Clients.All.SendAsync("product:deleted", id);
            return NoContent();
        }
    }
}
