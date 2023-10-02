﻿using LarsProjekt.Domain;

namespace LarsProjekt.Models.Mapping;

public static class OrderModelMappingExtension
{
    public static OrderModel ToModel(this Order order)
    {
        return new OrderModel
        {
            UserName = order.UserName,
            FirstName = order.FirstName,
            LastName = order.LastName,
            Phone = order.Phone,
            Email = order.Email,
            City = order.City,
            Country = order.Country,
            State = order.State,
            PostalCode = order.PostalCode,
            Total = order.Total,
            Date = order.Date,
            Id = order.Id,
            Address = order.Address
        };
    }

    public static Order ToDomain(this OrderModel model)
    {
        return new Order
        {
            UserName = model.UserName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.Phone,
            Email = model.Email,
            City = model.City,
            Country = model.Country,
            State = model.State,
            PostalCode = model.PostalCode,
            Total = model.Total,
            Date = model.Date,
            Id = model.Id,
            Address = model.Address
        };
    }
}
