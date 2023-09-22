using LarsProjekt.Application;
using LarsProjekt.Models;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers
{
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
                list.Add(new OrderModel
                {
                    OrderId = order.OrderId,
                    Username = order.Username,
                    OrderDate = order.OrderDate
                });
            }
            return View(list);
        }
    }
}
