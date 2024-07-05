using LarsProjekt.Domain.MyTemsApi;

namespace LarsProjekt.Domain.Interfaces
{
    public interface ICreateOrderService
    {
        Task CreateOrder(User user, Cart cart);
    }
}