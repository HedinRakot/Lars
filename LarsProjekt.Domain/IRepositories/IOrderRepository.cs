using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

public interface IOrderRepository
{
    void Add(Order order);
    void Delete(Order order);
    Order Get(long id);
    List<Order> GetAll();
    List<Order> GetOrdersById(long id);
    void Update(Order order);
    List<Order> GetOrderWithUser();
    List<Order> GetWithAddress();
}