using LarsProjekt.Domain.MyTemsApi;

namespace LarsProjekt.Domain.Interfaces;

public interface IOrderService
{
    Task Delete(long id);
    Task<Order> GetById(long id);
    Task<List<OrderDetail>> GetDetailListWithOrderId(long id);
    Task<List<Order>> Get();
    Task<Order> Update(Order order);
}