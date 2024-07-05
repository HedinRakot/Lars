namespace LarsProjekt.Dto.OrderApi.Mapping;

public static class OrderDtoMappingExtension
{
    public static Domain.OrderApi.Order ToDomain(this OrderDto dto)
    {
        var list = new List<Domain.OrderApi.OrderDetail>();
        foreach (var item in dto.Details)
        {
            list.Add(item.ToDomain());
        }

        return new Domain.OrderApi.Order
        {
            Id = dto.Id,
            OrderDate = dto.Date,
            Total = dto.Total,
            AddressId = dto.AddressId,
            CustomerId = dto.CustomerId,
            OrderDetails = list
        };
    }
    public static Domain.OrderApi.OrderDetail ToDomain(this OrderDetailDto dto)
    {
        return new Domain.OrderApi.OrderDetail
        {
            Id = dto.Id,
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            ProductAmount = dto.ProductAmount,
            UnitPrice = dto.UnitPrice
        };
    }

    public static OrderDto ToDto(this Domain.OrderApi.Order order)
    {
        var list = new List<OrderDetailDto>();
        foreach (var item in order.OrderDetails)
        {
            list.Add(item.ToDto());
        }
        return new OrderDto(
            order.Id,
            order.Total,
            order.AddressId,
            order.CustomerId,
            order.OrderDate,
            list);
    }

    public static OrderDetailDto ToDto(this Domain.OrderApi.OrderDetail detail)
    {
        return new OrderDetailDto(
            detail.Id,
            detail.OrderId,
            detail.ProductId,
            detail.ProductAmount,
            detail.UnitPrice);
    }
}
