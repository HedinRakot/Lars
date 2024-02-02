using LarsProjekt.Application;
using LarsProjekt.Database;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using LarsProjekt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LarsProjekt.Controllers;
public class OrderController : Controller
{
    private readonly ISqlUnitOfWork _sqlUnitOfWork;
    private readonly ILogger<OrderController> _logger;
    private readonly ICouponCountService _couponCountService;
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly IOrderDetailService _orderDetailService;
    private readonly IAddressService _addressService;
    private readonly IProductService _productService;
    public OrderController
        (ISqlUnitOfWork sqlUnitOfWork,
        ILogger<OrderController> logger,
        ICouponCountService couponCountService,
        IAddressService addressService,
        IUserService userService,
        IOrderDetailService orderDetailService,
        IOrderService orderService,
        IProductService productService)
    {
        _sqlUnitOfWork = sqlUnitOfWork;
        _logger = logger;
        _couponCountService = couponCountService;
        _orderDetailService = orderDetailService;
        _addressService = addressService;
        _orderService = orderService;
        _userService = userService;
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userService.GetByName(HttpContext.User.Identity.Name);
        var address = await _addressService.GetById(user.AddressId);
        var orders = await _orderService.Get();
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
    public async Task<IActionResult> Checkout()
    {
        var cart = GetCartModel();
        var user = await _userService.GetByName(HttpContext.User.Identity.Name);
        var address = await _addressService.GetById(user.AddressId);

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
        var total = GetTotal();
        var moneyDiscount = CalcMoneyDiscount();
        var discount = CalcDiscount();
        total = total - moneyDiscount - discount;

        return total;
    }
    private decimal CalcDiscount()
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
        var list = new List<string>();

        foreach (var item in cart.Offers)
        {
            if (item.Coupon.Type == "Percent")
            {
                list.Add(item.Coupon.Discount);
            }
        }
        var value = list.Sum(x => Convert.ToDecimal(x));
        return (value / 100) * total;
    }

    private decimal CalcMoneyDiscount()
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
        var list = new List<string>();

        foreach (var item in cart.Offers)
        {
            if (item.Coupon.Type == "Money")
            {
                list.Add(item.Coupon.Discount);
            }

        }
        var value = list.Sum(x => Convert.ToDecimal(x));
        return value;

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
    public async Task<IActionResult> Details(long id)
    {                
        var detail = await _orderDetailService.GetListWithOrderId(id);
        var list = new List<OrderDetailModel>();
        foreach (var item in detail)
        {
            var product = await _productService.GetById(item.ProductId);     // TESTEN

            list.Add(new OrderDetailModel
            {
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount,
                DiscountedPrice = item.DiscountedPrice,
                Id = id,
                Product = product.ToModel()
            });
        }

        return View(list);
    }


    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderModel model)
    {

        if (ModelState.IsValid)
        {
            var user = await _userService.GetByName(HttpContext.User.Identity.Name);
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
                var orderDetail = new OrderDetail();                
                orderDetail.Quantity = item.Amount;
                orderDetail.UnitPrice = item.PriceOffer;
                orderDetail.ProductId = item.ProductId;
                orderDetail.OrderId = order.Id;
                orderDetail.Discount = item.Discount;
                orderDetail.DiscountedPrice = item.DiscountedPrice;

                _sqlUnitOfWork.OrderDetailRepository.Add(orderDetail);
                _sqlUnitOfWork.SaveChanges();
            }
            
            foreach(var offer in cart.Offers)
            {
                _couponCountService.UpdateCouponCount(offer.Coupon.Code);
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


