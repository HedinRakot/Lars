using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

public interface IOrderDetailRepository
{
    void Add(OrderDetail orderDetail);
    void Delete(OrderDetail orderDetail);
    OrderDetail Get(long id);
    List<OrderDetail> GetAll();
    void Update(OrderDetail orderDetail);
}