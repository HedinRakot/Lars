using LarsProjekt.Application;
using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers;
public class ShoppingCartController : Controller
{
    private IProductRepository _productRepository;
    private ShoppingCartRepository _cartRepository;
    public ShoppingCartController(IProductRepository productRepository, ShoppingCartRepository cartRepository)
    {
        _productRepository = productRepository;
        _cartRepository = cartRepository;
    }

    [HttpGet]
    public IActionResult Index()
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
        return View(list);
    }


    [HttpGet]
    public IActionResult AddToCart(int id)
    {
        var product = _productRepository.Get(id);        
        var shoppingCartItem = _cartRepository.ShoppingCartItems.FirstOrDefault(x => x.Product.Id == id);
        if (shoppingCartItem == null)
        {
            var cartItem = new ShoppingCartItem
            {
                Amount = 1,
                Product = product
            };
            _cartRepository.ShoppingCartItems.Add(cartItem);
        }
        else
        {
            shoppingCartItem.Amount++;
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult AmountMinus(int id)
    {
        var item = _cartRepository.ShoppingCartItems.FirstOrDefault(p => p.Product.Id == id);
        if (item != null)
        {
            if (item.Amount > 1)
            {
                item.Amount--;
            }
            else
            {
                _cartRepository.ShoppingCartItems.Remove(item);
            }
        }
        return RedirectToAction(nameof(Index));
    }


    [HttpDelete]
    public IActionResult RemoveFromCart(int id)
    {
        var item = _cartRepository.ShoppingCartItems.FirstOrDefault(p => p.Product.Id == id);
        if (item != null)
        {
            if (item.Amount > 1)
            {
                item.Amount--;
            }
            else
            {
                _cartRepository.ShoppingCartItems.Remove(item);
            }
        }
        return Ok(new { success = "true" });
    }


    [HttpGet]
    public IActionResult EmptyCart()
    {
        var items = _cartRepository.ShoppingCartItems.ToList();
        foreach (var item in items)
        {
            _cartRepository.ShoppingCartItems.Remove(item);
        }        
        return RedirectToAction(nameof(Index));
    }
}