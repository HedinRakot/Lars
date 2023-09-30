﻿using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

internal class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Order> GetAll()
    {
        var orders = _context.Orders;
        return orders.ToList();
    }

    public void Add(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    public Order Get(long id)
    {
        return _context.Orders.FirstOrDefault(u => u.OrderId == id);
    }

    public void Update(Order order)
    {
        _context.Orders.Update(order);
        _context.SaveChanges();
    }

    public void Delete(Order order)
    {
        _context.Orders.Remove(order);
        _context.SaveChanges();
    }
}
