using LarsProjekt.Domain;

namespace LarsProjekt.Application;

public interface IProductService
{
    Task<List<Product>> GetProducts();
}