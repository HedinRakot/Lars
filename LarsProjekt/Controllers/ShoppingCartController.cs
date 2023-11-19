using LarsProjekt.Database.Repositories;
using LarsProjekt.Models;
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
    public IActionResult Index()
    {
        var models = new CartModel
        {
            Items = new List<ShoppingCartItemModel>()            
        };

        var user = HttpContext.User.Identity.Name;
        var cartId = GetCartId(HttpContext);
        if ( user == cartId )
        {
            var cookieValue = Request.Cookies["cookieCart"];

            if (!string.IsNullOrWhiteSpace(cookieValue))
            {
                models = JsonSerializer.Deserialize<CartModel>(cookieValue);
            }

        }

        return View(models);
    }


    [HttpGet]
    public IActionResult AddToCart(int id)
    {
        var product = _productRepository.Get(id);
        var user = HttpContext.User.Identity.Name;
        var cartId = GetCartId(HttpContext);

        var cart = new CartModel { Items = new List<ShoppingCartItemModel>() };

        var cookieValue = Request.Cookies["cookieCart"];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            cart = JsonSerializer.Deserialize<CartModel>(cookieValue);
        }
        if ( cart.ShoppingCartId == null || cart.ShoppingCartId != user)
        {
            cart.ShoppingCartId = user;
        }
        
        if ( cartId == cart.ShoppingCartId)
        {
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
            Response.Cookies.Append("cookieCart", newCookieValue, options);
        }
        

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult AmountMinus(int id)
    {
        var shoppingCartItems = new List<ShoppingCartItemModel>();
        var cookieValue = Request.Cookies["cookieShoppingCartItems"];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            shoppingCartItems = JsonSerializer.Deserialize<List<ShoppingCartItemModel>>(cookieValue);
        }

        var item = shoppingCartItems.FirstOrDefault(x => x.ProductId == id);

        if (item != null)
        {
            if (item.Amount > 1)
            {
                item.Amount--;
            }
            else
            {
                shoppingCartItems.Remove(item);
            }
        }
        var newCookieValue = JsonSerializer.Serialize(shoppingCartItems);
        Response.Cookies.Append("cookieShoppingCartItems", newCookieValue);

        return RedirectToAction(nameof(Index));
    }


    [HttpDelete]
    public IActionResult RemoveFromCart(int id)
    {
        var shoppingCartItems = new List<ShoppingCartItemModel>();
        var cookieValue = Request.Cookies["cookieShoppingCartItems"];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            shoppingCartItems = JsonSerializer.Deserialize<List<ShoppingCartItemModel>>(cookieValue);
        }

        var item = shoppingCartItems.FirstOrDefault(x => x.ProductId == id);
        if (item != null)
        {
            shoppingCartItems.Remove(item);
        }

        var newCookieValue = JsonSerializer.Serialize(shoppingCartItems);
        Response.Cookies.Append("cookieShoppingCartItems", newCookieValue);

        return Ok(new { success = "true" });
    }


    [HttpGet]
    public IActionResult EmptyCart()
    {
        var shoppingCartItems = new List<ShoppingCartItemModel>();
        var cookieValue = Request.Cookies["cookieShoppingCartItems"];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            shoppingCartItems = JsonSerializer.Deserialize<List<ShoppingCartItemModel>>(cookieValue);
        }

        var newCookieValue = JsonSerializer.Serialize(shoppingCartItems);
        var options = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(-1)
        };
        Response.Cookies.Append("cookieShoppingCartItems", newCookieValue, options);

        return RedirectToAction(nameof(Index));
    }

    private string GetCartId(HttpContext context)
    {
        string? user = context.User.Identity.Name;
        if (context.Session.GetString("CartSessionKey") == null)
        {
            if (!string.IsNullOrWhiteSpace(user))
            {
                context.Session.SetString("CartSessionKey", user);
            }
            else
            {
                throw new Exception();
            }

            // shopping für nicht registrierte nutzer
            //else
            //{
            //    Guid tempCartId = Guid.NewGuid();
            //    context.Session.SetString("CartSessionKey", tempCartId.ToString());
            //}
        }
        return context.Session.GetString("CartSessionKey").ToString();
    }

    //private List<ShoppingCartItemModel>? GetCartItems()
    //{
    //    var shoppingCartItems = new List<ShoppingCartItemModel>();
    //    var cookieValue = Request.Cookies["cookieShoppingCartItems"];

    //    if (!string.IsNullOrWhiteSpace(cookieValue))
    //    {
    //        shoppingCartItems = JsonSerializer.Deserialize<List<ShoppingCartItemModel>>(cookieValue);
    //    }
    //    return shoppingCartItems;
    //}
    //private CartModel GetCart(HttpContext context)
    //{
    //    var cart = new CartModel()
    //    {
    //        ShoppingCartId = GetCartId(context),
    //        Items = GetCartItems()
    //    };
    //    return cart;
    //}

}