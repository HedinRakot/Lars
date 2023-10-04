using LarsProjekt.Application;
using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LarsProjekt.Controllers;

public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly ShoppingCartRepository _cartRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IUserRepository _userRepository;

    public OrderController(IOrderRepository orderRepository,
        ShoppingCartRepository cartRepository,
        IOrderDetailRepository orderDetailRepository,
        IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _orderDetailRepository = orderDetailRepository;
        _userRepository = userRepository;
    }
    public IActionResult Index()
    {
        var list = new List<OrderModel>();
        foreach (var order in _orderRepository.GetAll())
        {
            list.Add(order.ToModel());
        }
        return View(list);
    }

    public IActionResult Checkout()
    {
        var list = new List<OrderModel>();
        foreach (var order in _orderRepository.GetAll())
        {
            list.Add(order.ToModel());
        }
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

    [HttpGet]
    public IActionResult Details(long id)
    {
        var detail = _orderDetailRepository.GetListWithOrderId(id);
        foreach (var item in detail)
        {
            var model = new OrderDetailModel
            {
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Id = id
            };
            return View(model);
        }
        return View();
    }

    //[HttpGet]
    //public IActionResult Details(long id)
    //{
    //    var detail = _orderDetailRepository.GetListWithOrderId(id);
    //    return View(detail);

    //}



    [HttpPost]
    public IActionResult CreateOrder(OrderModel model)
    {
        // error with id
        // überschreibt zeit bei jedem eintrag
        if (!ModelState.IsValid)
        {
            // add order
            var order = model.ToDomain();
            //model.UserId = order.User.Id;
            _orderRepository.Add(order);

            //add orderDetail
            // fehler beim schreiben in die db
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
}


//if (!ModelState.IsValid)
//        {
//            //decimal orderTotal = 0;

//            var order = model.ToDomain();
//_orderRepository.Add(order);

//            var cartItems = _cartRepository.ShoppingCartItems.ToList();
//            foreach (var item in cartItems)
//            {
//                var orderDetail = new OrderDetail
//                {
//                    Quantity = item.Amount,
//                    UnitPrice = item.Product.Price,
//                    ProductId = item.Product.Id,
//                    OrderId = 1
//                };
////orderTotal += (item.Amount * item.Product.Price);

//_orderDetailRepository.Add(orderDetail);                
//            }
//            //order.Total = orderTotal;
//            //unitofwork save

