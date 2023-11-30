using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
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


    // TODO Wenn Anzahl der Produkte im Warenkorb verändert wird, den Preis mit Rabatt anpassen.

    [HttpGet]
    public IActionResult AddToCart(int id)
    {
        var product = _productRepository.Get(id);
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cart = new CartModel();
        var cookieValue = Request.Cookies[cookie];
        var offers = GetOfferModels();

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            cart = JsonSerializer.Deserialize<CartModel>(cookieValue);
        }

        var shoppingCartItem = cart.Items.FirstOrDefault(x => x.ProductId == id);
        if (shoppingCartItem == null)
        {
            var cartItem = new ShoppingCartItemModel
            {
                Amount = 1,
                Name = product.Name,
                PriceOffer = product.PriceOffer,
                ProductId = id,
            };
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
                    Response.Cookies.Delete($"Offer{user}");
                }
            }
        }
        var newCookieValue = JsonSerializer.Serialize(cart);
        Response.Cookies.Append(cookie, newCookieValue);

        return RedirectToAction(nameof(Index));
    }


    [HttpDelete]
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
        }

        var newCookieValue = JsonSerializer.Serialize(cart);
        Response.Cookies.Append(cookie, newCookieValue);

        return Ok(new { success = "true" });
    }


    [HttpGet]
    public IActionResult EmptyCart()
    {
        var user = HttpContext.User.Identity.Name;
        
        Response.Cookies.Delete($"shoppingCart{user}");
        Response.Cookies.Delete($"Offer{user}");

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

        if (offers.Count() >= 3)
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