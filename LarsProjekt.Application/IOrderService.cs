using LarsProjekt.Domain;

namespace LarsProjekt.Application;

public interface IOrderService
{
    Task<Order> Create(Order order);
    Task<string> Delete(long id);
    Task<Order> GetById(long id);
    Task<List<Order>> Get();
    Task<Order> Update(Order order);
}