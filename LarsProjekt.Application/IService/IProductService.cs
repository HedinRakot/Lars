using LarsProjekt.Domain;
using LarsProjekt.Dto;

namespace LarsProjekt.Application.IService;

public interface IProductService
{
    Task<List<Product>> GetProducts();
    Task<Product> GetById(long id);
    Task<ProductDto> Update(Product product);
    Task<ProductDto> Create(Product product);
    Task Delete(long id);
}