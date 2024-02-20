using MyTemsAPI.Database;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Domain;

namespace MyTemsAPI.Repositories;

public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly AppDbContext _context;
    public OrderDetailRepository(AppDbContext context)
    {
        _context = context;
    }
    public List<OrderDetail> GetListWithOrderId(long id)
    {
        return _context.OrderDetails.Where(o => o.OrderId == id).ToList();
    }

    public void Add(OrderDetail orderDetail)
    {
        _context.OrderDetails.Add(orderDetail);
    }
}
