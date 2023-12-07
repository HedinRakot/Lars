using LarsProjekt.Database;
using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using LarsProjekt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace LarsProjekt.Controllers;
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly ISqlUnitOfWork _sqlUnitOfWork;
    private readonly ILogger<OrderController> _logger;
    
    public OrderController
        (IOrderRepository orderRepository,
        IOrderDetailRepository orderDetailRepository,
        IUserRepository userRepository,
        IAddressRepository addressRepository,
        ICouponRepository couponRepository,
        ISqlUnitOfWork sqlUnitOfWork,
        ILogger<OrderController> logger
        )
    {
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _userRepository = userRepository;
        _addressRepository = addressRepository;
        _couponRepository = couponRepository;
        _sqlUnitOfWork = sqlUnitOfWork;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var user = _userRepository.GetByName(HttpContext.User.Identity.Name);
        var address = _addressRepository.Get(user.AddressId);
        //var orders = _orderRepository.GetOrdersById(user.Id);
        
        //foreach (var order in orders)
        //{
        //    orders.Add(order);
        //    order.Address = address;
        //    order.ToModel();
        //}
        //return View(orders);

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
        var cart = GetCartModel();
        var user = _userRepository.GetByName(HttpContext.User.Identity.Name);
        var address = _addressRepository.Get(user.AddressId);

        cart.Total = CalcDiscountedTotal();       

        ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
        {
            Cart = cart,
            Address = address.ToModel(),
        };

        return View(shoppingCartVM);
    }

    private decimal CalcDiscountedTotal()
    {
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cart = new CartModel();
        var cookieValue = Request.Cookies[cookie];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            cart = JsonSerializer.Deserialize<CartModel>(cookieValue);
        }
        var total = GetTotal();
        var value = cart.Offers.Sum(x => Convert.ToDecimal(x.Coupon.Discount));
        var discount = (value / 100) * total;
        total -= discount;

        return total;
    }

    private decimal GetTotal()
    {
        var shoppingCartItems = GetCartModel().Items;

        decimal? total = (from cartItems in shoppingCartItems
                          select cartItems.Amount *
                          cartItems.PriceOffer).Sum();

        return total ?? 0;
    }

    [HttpGet]
    public IActionResult Details(long id)
    {
        var cart = GetCartModel();
        
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
                Id = id,
                ItemModel = cart.Items.FirstOrDefault(x => x.ProductId == item.ProductId),
                
            });
        }

        OrderVM vm = new OrderVM
        {            
            Details = list,
            Discount = cart.Discount,
            Offers = cart.Offers
        };

        return View(vm);
    }


    [HttpPost]
    public IActionResult CreateOrder(OrderModel model)
    {

        if (ModelState.IsValid)
        {
            var user = _userRepository.GetByName(HttpContext.User.Identity.Name);
            var cart = GetCartModel();
            var cookie = $"shoppingCart{HttpContext.User.Identity.Name}";
            
            // add order
            var order = model.ToDomain();
            order.UserId = user.Id;
            order.AddressId = user.AddressId;
            order.Total = cart.Total;
            _sqlUnitOfWork.OrderRepository.Add(order);
            _sqlUnitOfWork.SaveChanges();

            //add orderDetail            
            foreach (var item in cart.Items)
            {
                var orderDetail = new OrderDetail
                {
                    Quantity = item.Amount,
                    UnitPrice = item.PriceOffer,
                    ProductId = item.ProductId,
                    OrderId = order.Id,
                };
                _sqlUnitOfWork.OrderDetailRepository.Add(orderDetail);
                _sqlUnitOfWork.SaveChanges();
            }

            // clear cart
            Response.Cookies.Delete(cookie);
        }
        return RedirectToAction(nameof(Index));
    }

    private CartModel? GetCartModel()
    {
        var cart = new CartModel();
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cookieValue = Request.Cookies[cookie];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            cart = JsonSerializer.Deserialize<CartModel>(cookieValue);
        }
        return cart;
    }

}


