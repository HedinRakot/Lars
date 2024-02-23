using MyTemsAPI.Domain;

namespace MyTemsAPI.Application.IServices;

public interface ICreateOrderService
{
    Task CreateOrder(OrderEvent order);
}