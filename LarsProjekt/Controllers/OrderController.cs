using LarsProjekt.Application.IService;
using LarsProjekt.Domain;

using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using LarsProjekt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace LarsProjekt.Controllers;
public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;

    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly IOrderDetailService _orderDetailService;
    private readonly IAddressService _addressService;
    private readonly IProductService _productService;
    public OrderController
        (ILogger<OrderController> logger,

        IAddressService addressService,
        IUserService userService,
        IOrderDetailService orderDetailService,
        IOrderService orderService,
        IProductService productService)
    {
        _logger = logger;

        _orderDetailService = orderDetailService;
        _addressService = addressService;
        _orderService = orderService;
        _userService = userService;
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userService.GetByNameWithAddress(HttpContext.User.Identity.Name);        
        var orders = await _orderService.Get();
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
    public async Task<IActionResult> Checkout()
    {
        var cart = GetCartModel();
        var user = await _userService.GetByNameWithAddress(HttpContext.User.Identity.Name);

        //cart.Total = CalcDiscountedTotal();

        ShoppingCartVM shoppingCartVM = new()
        {
            Cart = cart,
            Address = user.Address.ToModel(),
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
    public async Task<IActionResult> CreateOrder()
    {             
        List<OrderDetail> list = new();
        CartModel model = GetCartModel();
        User user = await _userService.GetByName(HttpContext.User.Identity.Name);

        Order order = new()
        {
            Total = model.Total,
            Date = DateTime.Now,
            Details = list,
            AddressId = user.AddressId,
            UserId = user.Id
        };

        foreach (var item in model.Items)
        {
            list.Add(new OrderDetail
            {
                Quantity = item.Amount,
                UnitPrice = item.PriceOffer,
                ProductId = item.ProductId,
                OrderId = order.Id,
                Discount = item.Discount,
                DiscountedPrice = item.DiscountedPrice
            });
        }
        await _orderService.Create(order);

        Response.Cookies.Delete($"shoppingCart{HttpContext.User.Identity.Name}");
        return RedirectToAction(nameof(Index));

        //if (ModelState.IsValid)
        //{
        //    var user = await _userService.GetByNameWithAddress(HttpContext.User.Identity.Name);
        //    var cart = GetCartModel();
        //    var cookie = $"shoppingCart{HttpContext.User.Identity.Name}";
        //    var list = new List<OrderDetail>();

        //    Order order = new()
        //    {
        //        Total = cart.Total,
        //        Date = DateTime.Now,
        //        Details = list
        //    };

        //    foreach (var item in cart.Items)
        //    {
        //        list.Add(new OrderDetail
        //        {
        //            Quantity = item.Amount,
        //            UnitPrice = item.PriceOffer,
        //            ProductId = item.ProductId,
        //            OrderId = order.Id,
        //            Discount = item.Discount,
        //            DiscountedPrice = item.DiscountedPrice
        //        });
        //    }
        //    await _orderService.Create(order);

        //    Response.Cookies.Delete(cookie);
        //}
        //return RedirectToAction(nameof(Index));
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




// add order
//var order = model.ToDomain();
//order.UserId = user.Id;
//order.AddressId = user.AddressId;
//order.Total = cart.Total;
//await _orderService.Create(order);

//add orderDetail                                   
//foreach (var item in cart.Items)
//{
//    var orderDetail = new OrderDetail();
//    orderDetail.Quantity = item.Amount;
//    orderDetail.UnitPrice = item.PriceOffer;
//    orderDetail.ProductId = item.ProductId;
//    orderDetail.OrderId = order.Id;
//    orderDetail.Discount = item.Discount;
//    orderDetail.DiscountedPrice = item.DiscountedPrice;

//    await _orderDetailService.Create(orderDetail);
//}

//foreach (var offer in cart.Offers)
//{
//    _couponCountService.UpdateCouponCount(offer.Coupon.Code);
//}

// clear cart