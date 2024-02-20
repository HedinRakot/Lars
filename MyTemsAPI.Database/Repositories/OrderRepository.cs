using MyTemsAPI.Database;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Domain;

namespace MyTemsAPI.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Order> GetAll()
    {
        return _context.Orders.ToList();
    }

    public Order GetById(long id)
    {
        return _context.Orders.FirstOrDefault(x => x.Id == id);
    }

    public void Add(Order order)
    {
        _context.Orders.Add(order);
    }
    public void Update(Order order)
    {
        _context.Orders.Update(order);
        _context.SaveChanges();
    }
    public void Delete(Order order)
    {
        _context.Orders.Remove(order);
        _context.SaveChanges();
    }
}
