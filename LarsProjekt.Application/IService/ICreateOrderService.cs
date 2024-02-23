using LarsProjekt.Domain;

namespace LarsProjekt.Application.IService
{
    public interface ICreateOrderService
    {
        Task CreateOrder(User user, Cart cart);
    }
}