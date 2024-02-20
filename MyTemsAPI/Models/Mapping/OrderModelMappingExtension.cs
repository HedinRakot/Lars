using MyTemsAPI.Domain;

namespace MyTemsAPI.Models.Mapping;

public static class OrderModelMappingExtension
{    
    public static OrderModel ToModel(this Order order)
    {
        return new OrderModel
        {            
            Total = order.Total,
            Date = order.Date,
            Id = order.Id
        };
    }

    public static Order ToDomain(this OrderModel model)
    {
        return new Order
        {           
            Date = DateTimeOffset.Now            
        };
    }
}
