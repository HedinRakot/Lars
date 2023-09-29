using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

internal class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Product> GetAll()
    {
        var products = _context.Products;
        return products.ToList();
    }

    public void Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public Product Get(long id)
    {
        return _context.Products.FirstOrDefault(u => u.Id == id);
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    public void Delete(Product product)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
    }
}
