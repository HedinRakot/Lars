using LarsProjekt.Domain;

namespace LarsProjekt.Application;

public interface IOrderDetailService
{
    Task<string> Delete(long id);
    Task<List<OrderDetail>> Get();
    Task<OrderDetail> GetById(long id);
    Task<List<OrderDetail>> GetListWithOrderId(long id);
}