using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

public interface IProductRepository
{
    void Add(Product product);
    void Delete(Product product);
    Product Get(long id);
    List<Product> GetAll();
    void Update(Product product);
}