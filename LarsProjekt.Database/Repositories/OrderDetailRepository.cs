//using LarsProjekt.Domain;

//namespace LarsProjekt.Database.Repositories;

//internal class OrderDetailRepository : IOrderDetailRepository
//{
//    private readonly ApplicationDbContext _context;

//    public OrderDetailRepository(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public List<OrderDetail> GetAll()
//    {
//        var orderDetails = _context.OrderDetails;
//        return orderDetails.ToList();
//    }

//    public void Add(OrderDetail orderDetail)
//    {
//        _context.OrderDetails.Add(orderDetail);
//        _context.SaveChanges();
//    }

//    public OrderDetail Get(long id)
//    {
//        return _context.OrderDetails.FirstOrDefault(u => u.OrderDetailId == id);
//    }

//    public void Update(OrderDetail orderDetail)
//    {
//        _context.OrderDetails.Update(orderDetail);
//        _context.SaveChanges();
//    }

//    public void Delete(OrderDetail orderDetail)
//    {
//        _context.OrderDetails.Remove(orderDetail);
//        _context.SaveChanges();
//    }
//}
