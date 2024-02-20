

namespace MyTemsAPI.Domain.IRepositories
{
    public interface IOrderRepository
    {
        void Add(Order order);
        void Delete(Order order);
        List<Order> GetAll();
        Order GetById(long id);
        void Update(Order order);
    }
}