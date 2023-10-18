using LarsProjekt.Domain;

namespace LarsProjekt.Models.Mapping;

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
            Id = model.Id,
            
            Total = model.Total,
            Date = DateTimeOffset.Now,         
            
        };
    }
}
