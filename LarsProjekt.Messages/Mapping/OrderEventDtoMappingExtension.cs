using LarsProjekt.Domain;
using LarsProjekt.Messages.Dtos;

namespace LarsProjekt.Messages.Mapping;

public static class OrderEventDtoMappingExtension
{
    //public static OrderEventDto ToDto(this Order order)
    //{
    //    var list = new List<OrderDetailDto>();
    //    foreach (var item in order.Details)
    //    {
    //        list.Add(item.ToDto());
    //    }

    //    return new OrderDto(
    //        order.Id,
    //        order.Total,
    //        order.Date,
    //        order.AddressId,
    //        order.UserId,
    //        list
    //        );
    //}
    public static Order ToDomain(this OrderEventDto dto)
    {
        var details = new List<OrderDetail>();
        foreach (var item in dto.Details)
        {
            details.Add(item.ToDomain());
        }

        return new Order
        {
            Id = dto.Id,
            Date = dto.CreatedDate,
            Total = dto.Total,
            AddressId = dto.AddressId,
            UserId = dto.UserId,
            Details = details            
        };
    }
}
