using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LarsProjekt.Controllers;
public class ShoppingCartController : Controller
{
    private IProductRepository _productRepository;
    public ShoppingCartController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public IActionResult NewIndex()
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
        //var models = new CartModel();
        //var user = HttpContext.User.Identity.Name;
        //var cookie = $"shoppingCart{user}";
        //var cookieValue = Request.Cookies[cookie];

        //if (!string.IsNullOrWhiteSpace(cookieValue))
        //{
        //    models = JsonSerializer.Deserialize<CartModel>(cookieValue);
        //}
        //return View(models);
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