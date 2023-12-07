using LarsProjekt.Database.Repositories;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using LarsProjekt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LarsProjekt.Controllers;
public class ShoppingCartController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICouponRepository _couponRepository;
    public ShoppingCartController(IProductRepository productRepository, ICouponRepository couponRepository)
    {
        _productRepository = productRepository;
        _couponRepository = couponRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var cart = GetCartModel();
        cart.Total = CalcDiscountedTotal();

        if (cart.Offers.Count != 0)
        {
            cart.Discount = CalcDiscount();

            foreach (var item in cart.Offers)
            {
                decimal.TryParse(item.Coupon.Discount, out decimal x);

                item.DiscountedPrice = (x / 100) * GetTotal();
            }

            Response.Cookies.Append
                ($"shoppingCart{HttpContext.User.Identity.Name}",
                JsonSerializer.Serialize(cart),
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(1)
                });
        }

        ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
        {
            Cart = cart
        };

        return View(shoppingCartVM);

    }


    [HttpGet]
    public IActionResult AddToCart(int id)
    {
        var product = _productRepository.Get(id);
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cart = new CartModel();
        var cookieValue = Request.Cookies[cookie];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            cart = JsonSerializer.Deserialize<CartModel>(cookieValue);
        }

        var shoppingCartItem = cart.Items.FirstOrDefault(x => x.ProductId == id);
        var cartItem = new ShoppingCartItemModel();
        if (shoppingCartItem == null)
        {
            cartItem.Amount = 1;
            cartItem.Name = product.Name;
            cartItem.PriceOffer = product.PriceOffer;
            cartItem.ProductId = id;
            cartItem.Product = product.ToModel();
            cart.Items.Add(cartItem);
        }
        else
        {
            shoppingCartItem.Amount++;
        }

        var newCookieValue = JsonSerializer.Serialize(cart);
        var options = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(1)
        };
        Response.Cookies.Append(cookie, newCookieValue, options);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult AmountMinus(int id)
    {
        var cart = new CartModel();
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cookieValue = Request.Cookies[cookie];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            cart = JsonSerializer.Deserialize<CartModel>(cookieValue);
        }

        var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

        if (item != null)
        {
            if (item.Amount > 1)
            {
                item.Amount--;
            }
            else
            {
                cart.Items.Remove(item);

                if (cart.Items.Count == 0)
                {
                    Response.Cookies.Delete(cookie);
                }
            }
        }
        var newCookieValue = JsonSerializer.Serialize(cart);
        Response.Cookies.Append(cookie, newCookieValue);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public IActionResult RemoveFromCart(int id)
    {
        var cart = new CartModel();
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cookieValue = Request.Cookies[cookie];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            cart = JsonSerializer.Deserialize<CartModel>(cookieValue);
        }

        var item = cart.Items.FirstOrDefault(x => x.ProductId == id);
        if (item != null)
        {
            cart.Items.Remove(item);

            if (cart.Items.Count == 0)
            {
                Response.Cookies.Delete(cookie);
                return RedirectToAction(nameof(Index)); // diese Zeile entfernen, um die Coupons im Warenkorb zu lassen
            }
        }

        var newCookieValue = JsonSerializer.Serialize(cart);
        Response.Cookies.Append(cookie, newCookieValue);

        return RedirectToAction(nameof(Index));    
    }


    [HttpGet]
    public IActionResult EmptyCart()
    {
        var user = HttpContext.User.Identity.Name;

        Response.Cookies.Delete($"shoppingCart{user}");

        return RedirectToAction(nameof(Index));
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
        var value = cart.Offers.Sum(x => Convert.ToDecimal(x.Coupon.Discount));
        var discount = (value / 100) * total;

        return discount;
    }


    [HttpPost]
    public IActionResult Redeem(string couponCode)
    {
        var coupon = _couponRepository.Get(couponCode);
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cart = GetCartModel();
        var offer = cart.Offers.FirstOrDefault(x => x.Coupon.Code == couponCode);

        if(coupon != null)
        {
            if (cart.Offers.Count >= 3)
            {
                string message = "You can use a maximum of 3 codes per order.";
                return Json(new { success = "false", message, total = cart.Total });
            }

            if (offer == null)
            {
                decimal.TryParse(coupon.Discount, out decimal x);
                cart.Total = CalcDiscountedTotal();
                cart.Discount = CalcDiscount();
                if (cart.Discount == 0)
                {                    
                    cart.Discount = (x / 100) * cart.Total;
                    cart.Total = cart.Total - cart.Discount;
                }
                var model = new OfferModel
                {
                    Id = cart.Offers.Count > 0 ? cart.Offers.Max(o => o.Id) + 1 : 1,
                    Coupon = coupon.ToModel(),
                    DiscountedPrice = (x / 100) * GetTotal()
                };
                cart.Offers.Add(model);

                var newCookieValue = JsonSerializer.Serialize(cart);
                var options = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(1)
                };
                Response.Cookies.Append(cookie, newCookieValue, options);
                return Json(new { success = "true", total = cart.Total });
            }
            else
            {
                cart.Total = CalcDiscountedTotal();
                cart.Discount = CalcDiscount();
                string message = "This code can only be used once for an order.";
                return Json(new { success = "false", message, total = cart.Total });
            }
        }
        else
        {
            string message = "This code does not exist or cannot be redeemed.";
            return Json(new { success = "false", message, total = cart.Total });
        }

        //TODO in domain objekt auslagern
    }

    [HttpDelete]
    public IActionResult RemoveCoupon(long id)
    {
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cart = GetCartModel();

        var offer = cart.Offers.FirstOrDefault(x => x.Id == id);
        if (offer != null)
        {
            cart.Offers.Remove(offer);
        }

        var newCookieValue = JsonSerializer.Serialize(cart);
        Response.Cookies.Append(cookie, newCookieValue);
        return Ok(new { success = "true" });
    }

    private decimal GetTotal()
    {
        var shoppingCartItems = GetCartModel().Items;

        decimal? total = (from cartItems in shoppingCartItems
                          select cartItems.Amount *
                          cartItems.PriceOffer).Sum();

        return total ?? 0;
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