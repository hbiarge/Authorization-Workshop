using Products.Api.Models;

namespace Products.Api.Repositories
{
    public interface IProductsRepository
    {
        Product Get(string productNumber);
    }
}