using KitchenSync.Api.Models;

namespace KitchenSync.Api.Data
{
    public static class ProductSeed
    {
        public static IEnumerable<Product> All => new[]
        {
            new Product { Name = "Hambúrguer",      Category = "Lanches",         PrepSeconds = 420, Available = true },
            new Product { Name = "Batata Frita",    Category = "Acompanhamentos", PrepSeconds = 300, Available = true },
            new Product { Name = "Suco de Laranja", Category = "Bebidas",         PrepSeconds = 120, Available = true },
            new Product { Name = "Strogonoff",      Category = "Pratos",          PrepSeconds = 600, Available = true },
            // adicione mais aqui
        };
    }
}
