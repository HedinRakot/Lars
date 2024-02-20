

namespace MyTemsAPI.Domain.IRepositories
{
    public interface IOrderDetailRepository
    {
        void Add(OrderDetail orderDetail);
        List<OrderDetail> GetListWithOrderId(long id);
    }
}