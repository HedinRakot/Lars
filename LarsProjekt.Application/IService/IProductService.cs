using LarsProjekt.Domain;

namespace LarsProjekt.Application.IService;

public interface IProductService
{
    Task<List<Product>> GetProducts();
    Task<Product> GetById(long id);
    Task<Product> Update(Product product);
    Task<Product> Create(Product product);
    Task Delete(long id);
}