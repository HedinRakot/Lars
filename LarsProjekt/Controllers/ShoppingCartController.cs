using LarsProjekt.Application;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LarsProjekt.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ProductRepository _productRepository;
        private ShoppingCartRepository _shoppingCartRepository;
        public ShoppingCartController(ProductRepository productRepository, ShoppingCartRepository shoppingCartRepository)
        {
            _productRepository = productRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }
        public IActionResult Index()
        {
            var id = Guid.NewGuid().ToString();
            var list = new List<ShoppingCartItemModel>();

            foreach (var item in _shoppingCartRepository.ShoppingCartItems)
            {
                list.Add(new ShoppingCartItemModel
                {
                    Product = item.Product,
                    ShoppingCartId = id,
                    Amount = item.Amount
                });
            }
            //var cart = new ShoppingCartModel
            //{
            //    Items = list,
            //    ShoppingCartId = id,
            //};

            return View();

        }

        public IActionResult AddToCart(int id)
        {
            var product = _productRepository.Products.FirstOrDefault(p => p.Id == id);
            //var isFound = false;
            //foreach(var item in _shoppingCartRepository.ShoppingCartItems)
            //{
            //    if (id == item.Product.Id)
            //    {
            //        item.Amount++;
            //        isFound = true;
            //    }
            //}
            var shoppingCartItem = _shoppingCartRepository.ShoppingCartItems.FirstOrDefault(x => x.Product.Id == id);
            if (shoppingCartItem == null)
            //if (!isFound) 
            {
                var cartItem = new ShoppingCartItem
                {
                    ShoppingCartItemModelId = id,
                    Product = product                    
                };
                _shoppingCartRepository.ShoppingCartItems.Add(cartItem);
            } else
            {
                shoppingCartItem.Amount++;
            }

            return RedirectToAction(nameof(Index));

        }

        public IActionResult RemoveFromCart(int id)
        {
            var product = _shoppingCartRepository.ShoppingCartItems.FirstOrDefault(p => p.ShoppingCartItemModelId == id);
            if (product != null)
            {
                if (product.Amount > 1)
                {
                    product.Amount--;

                }
                else
                {
                    _shoppingCartRepository.ShoppingCartItems.Remove(product);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //public IActionResult EmptyCart(string id) // doesn't work
        //{
        //    var items = _shoppingCartRepository.ShoppingCarts.Where(c => c.ShoppingCartId == id);
        //    foreach (var item in items)
        //    {
        //        _shoppingCartRepository.ShoppingCarts.Remove(item);
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        //public decimal GetTotal()
        //{
        //    decimal? total = (from ShoppingCartItems in _itemRepository.ShoppingCartItems
        //                      where ShoppingCartId == ShoppingCartId
        //                      select (int?)ShoppingCartItems.Amount *
        //                      ShoppingCartItems.Product.Price).Sum();

        //    return total ?? decimal.Zero;
        //}
    }
}




//public void EmptyCart()
//{
//    var cartItems = _itemRepository.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId);
//    foreach (var cartItem in cartItems)
//    {
//        _itemRepository.ShoppingCartItems.Remove(cartItem);
//    }
//}
//public List<ShoppingCartItem> GetCartItems()
//{
//    return _itemRepository.ShoppingCartItems
//        .Where(c => c.ShoppingCartId == ShoppingCartId).ToList();
//}
//public decimal GetTotal()
//{
//    decimal? total = (from ShoppingCartItems in _itemRepository.ShoppingCartItems
//    where ShoppingCartId == ShoppingCartId
//                      select (int?)ShoppingCartItems.Amount *
//                      ShoppingCartItems.Product.Price).Sum();

//    return total ?? decimal.Zero;
//}