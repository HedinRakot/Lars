﻿using LarsProjekt.Application.IService;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using LarsProjekt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LarsProjekt.Controllers;
public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly IAddressService _addressService;
    private readonly IProductService _productService;
    private readonly ICreateOrderService _createOrderService;
    public OrderController
        (ILogger<OrderController> logger,
        IAddressService addressService,
        IUserService userService,
        IOrderService orderService,
        IProductService productService,
        ICreateOrderService createOrderService)
    {
        _logger = logger;
        _addressService = addressService;
        _orderService = orderService;
        _userService = userService;
        _productService = productService;
        _createOrderService = createOrderService;
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

        ShoppingCartVM shoppingCartVM = new()
        {
            Cart = cart,
            Address = user.Address.ToModel(),
        };
        return View(shoppingCartVM);
    }

    [HttpGet]
    public IActionResult PaypalPayment()
    {
        return View();
    }

    [HttpGet]
    public IActionResult InvoicePayment()
    {
        return View();
    }

    [HttpGet]
    public IActionResult OrderConfirmed()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Details(long id)
    {
        var detail = await _orderService.GetDetailListWithOrderId(id);
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

    public async Task<IActionResult> CreateOrder()
    {
        User user = await _userService.GetByName(HttpContext.User.Identity.Name);
        await _createOrderService.CreateOrder(user, GetCart());

        Response.Cookies.Delete($"shoppingCart{HttpContext.User.Identity.Name}");
        return RedirectToAction(nameof(OrderConfirmed));
    }

    private Cart? GetCart()
    {
        var cart = new Cart();
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cookieValue = Request.Cookies[cookie];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            cart = JsonSerializer.Deserialize<Cart>(cookieValue);
        }
        return cart;
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
}