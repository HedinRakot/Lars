using LarsProjekt.Domain;

namespace LarsProjekt.Dto.Mapping;

public static class OrderDtoMappingExtension
{
    public static OrderDto ToDto(this Order order)
    {
        var list = new List<OrderDetailDto>();
        foreach (var item in order.Details)
        {
            list.Add(item.ToDto());
        }

        return new OrderDto(
            order.Id,
            order.Total,
            order.Date,
            order.AddressId,
            order.UserId,
            list
            );
    }
    public static Order ToDomain(this OrderDto dto)
    {
        return new Order
        {
            Id = dto.Id,
            Date = dto.Date,
            Total = dto.Total,
            AddressId = dto.AddressId,
            UserId = dto.UserId
        };
    }
}
