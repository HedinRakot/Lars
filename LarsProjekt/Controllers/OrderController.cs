using LarsProjekt.Application;
using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LarsProjekt.Controllers;
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly ShoppingCartRepository _cartRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAddressRepository _addressRepository;

    public OrderController
        (IOrderRepository orderRepository,
        ShoppingCartRepository cartRepository,
        IOrderDetailRepository orderDetailRepository,
        IUserRepository userRepository,
        IAddressRepository addressRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _orderDetailRepository = orderDetailRepository;
        _userRepository = userRepository;
        _addressRepository = addressRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var user = _userRepository.GetByName(HttpContext.User.Identity.Name);
        var address = _addressRepository.Get(user.AddressId);
        var orders = _orderRepository.GetAll();
        var list = new List<OrderModel>();
        foreach (var order in orders)
        {
            if (user.Id == order.UserId)
            {                
                list.Add(order.ToModel());
                order.Address = address;
                
            }                
        }
        return View(list);
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        var user = _userRepository.GetByName(HttpContext.User.Identity.Name);        
        var model = _addressRepository.Get(user.AddressId).ToModel();        
        return View(model);
    }

    [HttpGet]
    public IActionResult Confirmation()
    {
        var list = new List<ShoppingCartItemModel>();
        foreach (var item in _cartRepository.ShoppingCartItems)
        {
            list.Add(new ShoppingCartItemModel
            {
                Product = item.Product.ToModel(),
                Amount = item.Amount
            });
        }
        return View(list);
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
            order.AddressId = user.AddressId;
            order.Address = user.Address;
            order.Total = GetTotal();
            _orderRepository.Add(order);

            //add orderDetail            
            var cartItems = _cartRepository.ShoppingCartItems.ToList();
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Quantity = item.Amount,
                    UnitPrice = item.Product.PriceOffer,
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

    private decimal? GetTotal()
    {
        decimal? total = (from cartItems in _cartRepository.ShoppingCartItems
                          select cartItems.Amount *
                          cartItems.Product.Price).Sum();

        return total ?? decimal.Zero;
    }
}


