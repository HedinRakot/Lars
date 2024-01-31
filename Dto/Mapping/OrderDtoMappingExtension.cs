using LarsProjekt.Domain;

namespace LarsProjekt.Dto.Mapping;

public static class OrderDtoMappingExtension
{
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto(
            order.Id,
            order.Total,
            order.Date
            );
    }
    public static Order ToDomain(this OrderDto dto)
    {
        return new Order
        {
            Id = dto.Id,
            Date = dto.Date,
            Total = dto.Total
        };
    }
}
