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
        var models = new List<ShoppingCartItemModel>();
        var cookieValue = Request.Cookies["cookieShoppingCartItems"];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            models = JsonSerializer.Deserialize<List<ShoppingCartItemModel>>(cookieValue);
        }
        return View(models);
    }


    [HttpGet]
    public IActionResult AddToCart(int id)
    {
        var product = _productRepository.Get(id);

        var shoppingCartItems = new List<ShoppingCartItemModel>();
        var cookieValue = Request.Cookies["cookieShoppingCartItems"];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            shoppingCartItems = JsonSerializer.Deserialize<List<ShoppingCartItemModel>>(cookieValue);
        }

        var shoppingCartItem = shoppingCartItems.FirstOrDefault(x => x.ProductId == id);
        if(shoppingCartItem == null) 
        {
            var cartItem = new ShoppingCartItemModel
            {
                Amount = 1,
                Name = product.Name,
                PriceOffer = product.PriceOffer,
                ProductId = id,
            };
            shoppingCartItems.Add(cartItem);
        }
        else
        {
            shoppingCartItem.Amount++;
        }

        var newCookieValue = JsonSerializer.Serialize(shoppingCartItems);
        var options = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(1)            
        };
        Response.Cookies.Append("cookieShoppingCartItems", newCookieValue, options);

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

}