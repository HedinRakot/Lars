using LarsProjekt.Application;
using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers;
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly ShoppingCartRepository _cartRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IUserRepository _userRepository;

    public OrderController
        (IOrderRepository orderRepository,
        ShoppingCartRepository cartRepository,
        IOrderDetailRepository orderDetailRepository,
        IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _orderDetailRepository = orderDetailRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var user = _userRepository.GetByName(HttpContext.User.Identity.Name);
        var orders = _orderRepository.GetAll();
        var list = new List<OrderModel>();
        foreach (var order in orders)
        {
            if (user.Id == order.UserId)
            {
                list.Add(order.ToModel());                
            }                
        }
        return View(list);
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        var model = new OrderModel(){};
        return View(model);
    }


    [HttpGet]
    public IActionResult Details(long id)
    {
        var detail = _orderDetailRepository.GetListWithOrderId(id);
        var list = new List<OrderDetailModel>();
        foreach (var item in detail)
        {
            list.Add(new OrderDetailModel
            {
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Id = id
            });
        }
        return View(list);
    }


    [HttpPost]
    public IActionResult CreateOrder(OrderModel model)
    {
        
        if (ModelState.IsValid)
        {
            var user = _userRepository.GetByName(HttpContext.User.Identity.Name);

            // add order
            var order = model.ToDomain();
            order.UserId = user.Id;
            _orderRepository.Add(order);

            //add orderDetail            
            var cartItems = _cartRepository.ShoppingCartItems.ToList();
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Quantity = item.Amount,
                    UnitPrice = item.Product.Price,
                    ProductId = item.Product.Id,
                    OrderId = order.Id

                };
                _orderDetailRepository.Add(orderDetail);

            }

            // clear cart
            var items = _cartRepository.ShoppingCartItems.ToList();
            foreach (var item in items)
            {
                _cartRepository.ShoppingCartItems.Remove(item);
            }

        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Payment()
    {
        return View();
    }

	private List<UserModel> GetUserModels()
	{
		var users = _userRepository.GetAll();
		var userModels = new List<UserModel>();
		foreach (var user in users)
		{
			userModels.Add(user.ToModel());
		}

		return userModels;
	}

}


