using System.Collections.Generic;
using System.Linq;
using Products.Api.Models;

namespace Products.Api.Repositories
{
    public class InMemoryProductsRepository : IProductsRepository
    {
        private readonly List<Product> _products = new List<Product>
        {
            new Product
            {
                ProductNumber = "1234",
                OwnerId = "HB04356",
                ProductName = "Clean code"
            },
            new Product
            {
                ProductNumber = "4321",
                OwnerId = "RG95478",
                ProductName = "Clean code"
            },
        };

        public Product Get(string productNumber)
        {
            return _products.FirstOrDefault(p => p.ProductNumber == productNumber);
        }
    }
}