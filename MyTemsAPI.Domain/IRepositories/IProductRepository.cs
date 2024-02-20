
namespace MyTemsAPI.Domain.IRepositories;

public interface IProductRepository
{
    void Add(Product product);
    void Delete(Product product);
    List<Product> GetAll();
    Product GetById(long id);
    void Update(Product product);
}