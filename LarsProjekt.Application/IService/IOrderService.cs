using LarsProjekt.Domain;

namespace LarsProjekt.Application.IService;

public interface IOrderService
{
    Task<Order> Create(Order order);
    Task Delete(long id);
    Task<Order> GetById(long id);
    Task<List<Order>> Get();
    Task<Order> Update(Order order);
}