using LarsProjekt.Domain;
using LarsProjekt.Dto;

namespace LarsProjekt.Application;

public interface IProductService
{
    Task<List<Product>> GetProducts();
    Task<Product> GetById(long id);
    Task<Product> Update(ProductDto productDto);
    Task<Product> Create(Product product);
    void Delete(long id);
}