using LarsProjekt.Application;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers;

public class OrderController : Controller
{
    private OrderRepository _orderRepository;
    public OrderController(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public IActionResult Index()
    {
        var list = new List<OrderModel>();
        foreach (var order in _orderRepository.Orders)
        {
            list.Add(order.ToModel());
        }
        return View(list);
    }

    public IActionResult Checkout()
    {
        var list = new List<OrderModel>();
        foreach(var order in _orderRepository.Orders)
        {
            list.Add(order.ToModel());
        }
        return View();
    }

    public IActionResult Payment()
    {
        return View();  
    }
}
