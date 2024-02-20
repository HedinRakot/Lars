using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Repositories;

namespace MyTemsAPI.Database;

internal class SqlUnitOfWork : ISqlUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;

    public SqlUnitOfWork(AppDbContext context)
    {
        _context = context;
        _orderRepository = new OrderRepository(context);
        _orderDetailRepository = new OrderDetailRepository(context);
    }
    public IOrderRepository OrderRepository => _orderRepository;
    public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
