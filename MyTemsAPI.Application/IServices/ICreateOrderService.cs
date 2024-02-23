using LarsProjekt.Messages.Dtos;
using MyTemsAPI.Domain;

namespace MyTemsAPI.Application.IServices;

public interface ICreateOrderService
{
    Task CreateOrder(OrderEventDto order);
}