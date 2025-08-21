using System.Collections.Concurrent;
using KitchenSync.Api.Models;

namespace KitchenSync.Api.Services
{
    public class InMemoryStore
    {
        private readonly ConcurrentDictionary<Guid, Product> _products = new();
        private readonly ConcurrentDictionary<Guid, Order> _orders = new();

        public InMemoryStore()
        {
            // Seed inicial
            var p1 = new Product { Name = "Hambúrguer", Category = "Lanches", PrepSeconds = 420, Available = true };
            var p2 = new Product { Name = "Batata Frita", Category = "Acompanhamentos", PrepSeconds = 300, Available = true };
            var p3 = new Product { Name = "Suco de Laranja", Category = "Bebidas", PrepSeconds = 120, Available = true };
            var p4 = new Product { Name = "Strogonoff", Category = "Pratos", PrepSeconds = 600, Available = true };

            _products[p1.Id] = p1; _products[p2.Id] = p2; _products[p3.Id] = p3; _products[p4.Id] = p4;

            
        }

        // PRODUCTS
        public IEnumerable<Product> GetProducts() => _products.Values.OrderBy(p => p.Name);
        public Product? GetProduct(Guid id) => _products.TryGetValue(id, out var p) ? p : null;
        public Product AddProduct(Product p) { _products[p.Id] = p; return p; }
        public bool UpdateProduct(Product p) { if (!_products.ContainsKey(p.Id)) return false; _products[p.Id] = p; return true; }
        public bool DeleteProduct(Guid id) => _products.TryRemove(id, out _);

        // ORDERS
        public IEnumerable<Order> GetOrders() => _orders.Values.OrderByDescending(o => o.CreatedAt);
        public Order AddOrder(Order o) { _orders[o.Id] = o; return o; }
        public Order? GetOrder(Guid id) => _orders.TryGetValue(id, out var o) ? o : null;
        public bool UpdateOrderStatus(Guid id, OrderStatus status)
        {
            if (!_orders.TryGetValue(id, out var o)) return false;
            o.Status = status; return true;
        }
    }
}
