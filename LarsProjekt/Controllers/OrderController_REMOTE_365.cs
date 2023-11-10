using LarsProjekt.Application;
using LarsProjekt.Database;
using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using LarsProjekt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Collections.Generic;

namespace LarsProjekt.Controllers;
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly ShoppingCartRepository _cartRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly ISqlUnitOfWork _sqlUnitOfWork;
    //private readonly DiscountPriceRepository _discountPriceRepository;

    public OrderController
        (IOrderRepository orderRepository,
        ShoppingCartRepository cartRepository,
        IOrderDetailRepository orderDetailRepository,
        IUserRepository userRepository,
        IAddressRepository addressRepository,
        ICouponRepository couponRepository,
        ISqlUnitOfWork sqlUnitOfWork)
    //DiscountPriceRepository discountPriceRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _orderDetailRepository = orderDetailRepository;
        _userRepository = userRepository;
        _addressRepository = addressRepository;
        _couponRepository = couponRepository;
        _sqlUnitOfWork = sqlUnitOfWork;
        //_discountPriceRepository = discountPriceRepository;
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

        OrderConfirmationVM orderConfirmationVM = new OrderConfirmationVM()
        {
            Coupons = new CouponModel(),
            Items = list,
            Total = GetTotal()
        };

        return View(orderConfirmationVM);
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
            _sqlUnitOfWork.OrderRepository.Add(order);
            _sqlUnitOfWork.SaveChanges();

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
                _sqlUnitOfWork.OrderDetailRepository.Add(orderDetail);
                _sqlUnitOfWork.SaveChanges();
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

    [HttpPost]
    public IActionResult Redeem(string couponCode)
    {
        var coupons = _couponRepository.GetAll() //TODO eine Query bauen beim DB Aufruf  
            .Where(c => c.Code == couponCode);

        //TODO in domain objekt auslagern
        var total = GetTotal();

        if (total > 0)
        {
            
            foreach (var coupon in coupons)
            {
                var discount = coupon.Discount;
                decimal.TryParse(discount, out decimal x);
                var totalDiscount = (x / 100) * total;
                var newTotalPrice = total - totalDiscount;
                total = newTotalPrice;
            }
        }

        //Coupon codes speichern...

        return Json(new { success = "true", total = total });
    }

    //[HttpGet]
    //public IActionResult Redeem()
    //{
    //    return Ok(new { success = "true", newPrice = _discountPriceRepository.Price.PriceAfterDiscount });
    //}

    public IActionResult Payment()
    {
        return View();
    }

    private decimal GetTotal()
    {
        decimal? total = (from cartItems in _cartRepository.ShoppingCartItems
                          select cartItems.Amount *
                          cartItems.Product.PriceOffer).Sum();

        return total ?? 0;
    }

}


