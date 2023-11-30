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
        ILogger<OrderController> logger)
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
        var cart = GetCartModel();        
        var offers = GetOfferModels();
        var total = GetTotal();

        if (offers.Count != 0)
        {
            total = offers.MinBy(x => x.DiscountedPrice).DiscountedPrice;
        }

        OrderConfirmationVM orderConfirmationVM = new OrderConfirmationVM()
        {
            Coupons = new CouponModel(),    
            Cart = cart,            
            Total = total,
            Offers = offers
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
            var cart = GetCartModel();
            var offers = GetOfferModels();
            var total = GetTotal();
            var cookieCart = $"shoppingCart{user}";
            var cookieOffer = $"Offer{user}";


            if (offers.Count != 0)
            {
                total = offers.MinBy(x => x.DiscountedPrice).DiscountedPrice;
            }
            // add order
            var order = model.ToDomain();
            order.UserId = user.Id;
            order.AddressId = user.AddressId;
            order.Address = user.Address;
            order.Total = total;
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
                    OrderId = order.Id
                };
                _sqlUnitOfWork.OrderDetailRepository.Add(orderDetail);
                _sqlUnitOfWork.SaveChanges();
            }

            // clear cart
            var newCookieValue = JsonSerializer.Serialize(cart);
            var newCookieValueOffer = JsonSerializer.Serialize(offers);
            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1)
            };
            Response.Cookies.Append(cookieCart, newCookieValue, options);
            Response.Cookies.Append(cookieOffer, newCookieValueOffer, options);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Redeem(string couponCode)
    {
        Coupon coupon = _couponRepository.Get(couponCode);
        decimal total = GetDiscountPrice();
        decimal totalDiscount = 0;
        List<OfferModel>? offers = GetOfferModels();
        var user = HttpContext.User.Identity.Name;
        var cookieOffer = $"Offer{user}";

        var offer = offers.FirstOrDefault(x => x.CouponCode == couponCode);

        if(offers.Count() >= 3)
        {
            var discountPrice = GetDiscountPrice();
            string message = "You can use a maximum of 3 codes per order.";
            return Json(new { success = "false", message = message, discountPrice = discountPrice });
        }
        if (offer == null)
        {
            if (total > 0)
            {
                var discount = coupon.Discount;
                decimal.TryParse(discount, out decimal x);
                totalDiscount = (x / 100) * total;
                var newTotalPrice = total - totalDiscount;
                total = newTotalPrice;
            }
            var model = new OfferModel
            {
                CouponCode = couponCode,
                DiscountedPrice = total,
                DiscountValue = coupon.Discount,
                Discount = totalDiscount,
                Id = offers.Count > 0 ? offers.Max(o => o.Id) + 1 : 1
            };
            offers.Add(model);
            var newCookieValue = JsonSerializer.Serialize(offers);
            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(1)
            };
            Response.Cookies.Append(cookieOffer, newCookieValue, options);

            return Json(new { success = "true", total = total });
        }
        else
        {
            var discountPrice = GetDiscountPrice();
            string message = "This code can only be used once for an order.";
            return Json(new { success = "false", message = message, discountPrice = discountPrice });
        }
        
        //TODO in domain objekt auslagern
    }

    [HttpDelete]
    public IActionResult RemoveCoupon(long id)
    {
        var offers = GetOfferModels();
        var user = HttpContext.User.Identity.Name;
        var cookieOffer = $"Offer{user}";

        var offer = offers.FirstOrDefault(x => x.Id == id);
        if (offer != null)
        {
            offers.Remove(offer);
        }

        var newCookieValue = JsonSerializer.Serialize(offers);
        Response.Cookies.Append(cookieOffer, newCookieValue);
        return Ok(new { success = "true"});
    }

    public IActionResult Payment()
    {
        return View();
    }

    private decimal GetTotal()
    {
        var shoppingCartItems = GetCartModel().Items;

        decimal? total = (from cartItems in shoppingCartItems
                          select cartItems.Amount *
                          cartItems.PriceOffer).Sum();

        return total ?? 0;
    }

    private decimal GetDiscountPrice()
    {
        var offers = GetOfferModels();
        var total = GetTotal();
        var y = offers.MinBy(x => x.DiscountedPrice);
        if (y != null)
        {
            total = y.DiscountedPrice;
        }
        return total;
    }

    private List<OfferModel>? GetOfferModels()
    {
        var offers = new List<OfferModel>();
        var user = HttpContext.User.Identity.Name;
        var cookie = $"Offer{user}";
        var cookieValue = Request.Cookies[cookie];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            offers = JsonSerializer.Deserialize<List<OfferModel>>(cookieValue);
        }
        return offers;
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


