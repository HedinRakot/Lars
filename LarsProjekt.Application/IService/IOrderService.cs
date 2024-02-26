using LarsProjekt.Domain;
using LarsProjekt.Dto;

namespace LarsProjekt.Application.IService;

public interface IOrderService
{
    Task<PlaceOrderDto> Create(PlaceOrderDto order);
    Task Delete(long id);
    Task<Order> GetById(long id);
    Task<List<OrderDetail>> GetDetailListWithOrderId(long id);
    Task<List<Order>> Get();
    Task<Order> Update(Order order);
}