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
            // Helpers
            void Add(
                string name,
                string category,
                bool isRecurring = false,
                DishType type = DishType.Prato,
                int prepSeconds = 60,
                bool available = true,
                IEnumerable<string>? tags = null
            )
            {
                var p = new Product
                {
                    Id = Guid.NewGuid(),              // <-- Guid
                    Name = name,
                    Category = category,
                    IsRecurring = isRecurring,
                    Type = type,
                    PrepSeconds = prepSeconds,
                    Available = available,
                    Tags = tags?.ToList() ?? new List<string>()
                };
                _products[p.Id] = p;                 // chave Guid
            }

            // Categorias
            const string PRATOS = "Pratos";
            const string CARNES = "Carnes";
            const string ACOMP = "Acompanhamentos";
            const string GUARN = "Guarnições";
            const string FRITURAS = "Frituras";
            const string SALADAS = "Saladas";
            const string CALDOS = "Caldos/Ensopados";
            const string MASSAS = "Massas";
            const string ESPECIAIS = "Especiais";

            // Seed (resumo do que combinamos)
            Add("FRANGO C/FRITAS", PRATOS);
            Add("LINGUIÇA DE PORCO C/FRITAS", PRATOS, tags: new[] { "linguiça cofril", "cofril" });
            Add("CARNE DE BOI C/ FRITAS OU AIPI", PRATOS, tags: new[] { "bovina", "aipim", "mandioca" });
            Add("CAMARAO C/ FRITAS OU AIPIM", PRATOS, tags: new[] { "camarao", "aipim", "mandioca" });

            // Frituras (porções)
            Add("BATATA FRITA", FRITURAS, type: DishType.Porcao, tags: new[] { "fritas", "batata" });
            Add("AIPIM FRITO", FRITURAS, type: DishType.Porcao, tags: new[] { "mandioca" });
            Add("POLENTA FRITA", FRITURAS, type: DishType.Porcao);
            Add("PALITO DE QUEIJO", FRITURAS, type: DishType.Porcao);
            Add("TORRESMO", FRITURAS, type: DishType.Porcao);
            Add("BANANA FRITA", FRITURAS, type: DishType.Porcao);
            Add("BATATA DOCE", FRITURAS, type: DishType.Porcao);
            Add("BERIGENLA FRITA", FRITURAS, type: DishType.Porcao, tags: new[] { "berinjela" });
            Add("BETERRABA FRITA", FRITURAS, type: DishType.Porcao);
            Add("ANEIS DE CEBOLA", FRITURAS, isRecurring: true, type: DishType.Porcao, tags: new[] { "cebola frita" });
            Add("COUVE FLOR FRITA", FRITURAS, type: DishType.Porcao);
            Add("OVO FRITO", FRITURAS, type: DishType.Porcao);
            Add("PASTEL FRITO", FRITURAS, isRecurring: true, type: DishType.Porcao);
            Add("PEIXE FRITO", FRITURAS, type: DishType.Porcao);
            Add("FILE DE PEIXE", FRITURAS, type: DishType.Porcao);
            Add("SALGADINHOS", FRITURAS, type: DishType.Porcao);
            Add("MINITICKEN", FRITURAS);

            // Caldos / panela
            Add("CALDO VERDE", CALDOS);
            Add("DOBRADINHA", CALDOS);
            Add("FEIJOADA", CALDOS);
            Add("TUTU", CALDOS);
            Add("BUCHO", CALDOS);
            Add("MOCOTO", CALDOS);
            Add("BOBO DE CAMARÃO", CALDOS, tags: new[] { "bobó de camarao" });
            Add("SURURU", CALDOS);

            // Massas / forno
            Add("LASANHA", MASSAS);
            Add("ESCONDIDINHO", PRATOS);
            Add("FRICASSE", PRATOS);
            Add("EMPADÃO", PRATOS);
            Add("TORTA CAPIXABA", ESPECIAIS);

            // Guarnições / básicas
            Add("PURE", GUARN);
            Add("POLENTA", GUARN);
            Add("MACARRÃO", MASSAS, isRecurring: true, tags: new[] { "massa" });

            // Carnes
            Add("COSTELA DE BOI", CARNES);
            Add("COSTELA DE PORCO", CARNES);
            Add("RABADA", CARNES);
            Add("LINGUA", CARNES);
            Add("CARNE ASSADA", CARNES);
            Add("BIFE DE BOI", CARNES);
            Add("BIFE DE PORCO", CARNES);
            Add("CUPIM ASSADO", CARNES);
            Add("LAGARTO ASSADO", CARNES);
            Add("PERNIL DE PORCO", CARNES);
            Add("LINGUIÇA ASSADA", CARNES, type: DishType.Porcao);
            Add("LINGUIÇA CASEIRA", CARNES, isRecurring: true, type: DishType.Porcao, tags: new[] { "linguiça cofril", "cofril" });
            Add("GALO", CARNES, tags: new[] { "galinha caipira", "capote" });
            Add("FRANGO EMPANADO", CARNES, isRecurring: true, type: DishType.Porcao);
            Add("BIFE DE FRANGO", CARNES);
            Add("FRANGO ASSADO", CARNES);
            Add("MOELA", CARNES, type: DishType.Porcao);
            Add("OVERA", CARNES);
            Add("CHORISCO", CARNES, type: DishType.Porcao, tags: new[] { "chouriço" });

            // Guarnições quentes / refogados
            Add("QUIABO FRITO", GUARN, type: DishType.Porcao);
            Add("QUIABO PRO GALO", GUARN);
            Add("CHUCHU REFOGADO", GUARN);
            Add("ABOBORA REFOGADA", GUARN);
            Add("JILO REFOGADO", GUARN);
            Add("COUVE REFOGADA", GUARN);

            // Acompanhamentos (recorrentes)
            Add("BATATA", ACOMP);
            Add("BATATA BOLINHA", ACOMP);
            Add("BATATA RUSTICA", ACOMP);
            Add("AIPIM", ACOMP, tags: new[] { "mandioca" });
            Add("ARROZ", ACOMP, isRecurring: true);
            Add("FEIJÃO CALDO", ACOMP, isRecurring: true, tags: new[] { "feijão", "feijao" });
            Add("FEIJÃO TROPEIRO", ACOMP, isRecurring: true);

            // Saladas e frios
            Add("MAIONESE MOLHO", SALADAS);
            Add("FAROFA", ACOMP);
            Add("SALPICAO", SALADAS);
            Add("SALADA DE MAIONESE", SALADAS);
            Add("SALADA DE VERDURA CRUA", SALADAS);
            Add("MACARRONESE", SALADAS);
            Add("SALADA DE ALHO PORO", SALADAS);
            Add("VINAGRETE", SALADAS);
            Add("SALADA MALUCA", SALADAS);
            Add("BROCOLIS", SALADAS);
            Add("COUVER-FLOR", SALADAS, tags: new[] { "couve-flor" });
            Add("BETERRABA", SALADAS);
            Add("CENOURA", SALADAS);
            Add("REBOLO ROXO", SALADAS, tags: new[] { "repolho roxo" });
            Add("REBOLO BRANCO", SALADAS, tags: new[] { "repolho" });
            Add("ALFACE", SALADAS);
            Add("TOMATE", SALADAS);
            Add("PEPINO", SALADAS);
            Add("MANGA", SALADAS);
            Add("ABACAXI", SALADAS);
            Add("PIMENTA BIQUINHO", SALADAS);
            Add("OVO DE CODORNA", SALADAS);
            Add("AZEITONA", SALADAS);
            Add("PALMITO", SALADAS);

            // Especiais
           
        }

        // PRODUCTS
        public IEnumerable<Product> GetProducts() => _products.Values.OrderBy(p => p.Name);
        public Product? GetProduct(Guid id) => _products.TryGetValue(id, out var p) ? p : null;
        public Product AddProduct(Product p) { if (p.Id == Guid.Empty) p.Id = Guid.NewGuid(); _products[p.Id] = p; return p; }
        public bool UpdateProduct(Product p) { if (!_products.ContainsKey(p.Id)) return false; _products[p.Id] = p; return true; }
        public bool DeleteProduct(Guid id) => _products.TryRemove(id, out _);

        // ORDERS (inalterado)
        private readonly object _ordersLock = new(); // se precisar garantir consistência
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
