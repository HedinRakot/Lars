using LarsProjekt.Domain.MyTemsApi;

namespace LarsProjekt.Domain.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetProducts();
    Task<Product> GetById(long id);
    Task<Product> Update(Product product);
    Task<Product> Create(Product product);
    Task Delete(long id);
}