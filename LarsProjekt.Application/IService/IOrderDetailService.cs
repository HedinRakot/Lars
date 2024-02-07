using LarsProjekt.Domain;

namespace LarsProjekt.Application.IService;

public interface IOrderDetailService
{
    Task Delete(long id);
    Task<List<OrderDetail>> Get();
    Task<OrderDetail> GetById(long id);
    Task<List<OrderDetail>> GetListWithOrderId(long id);
    Task<OrderDetail> Update(OrderDetail orderDetail);
    Task<OrderDetail> Create(OrderDetail orderDetail);
}