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
        private ShoppingCartItemRepository _shoppingCartItemRepository;
        public ShoppingCartController(ProductRepository productRepository, ShoppingCartRepository shoppingCartRepository, ShoppingCartItemRepository shoppingCartItemRepository) 
        {
            _productRepository = productRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
        }
        public IActionResult Index()
        {
            var list = new List<ShoppingCartItemModel>();

            foreach (var item in _shoppingCartItemRepository.ShoppingCartItems)
            {
                list.Add(new ShoppingCartItemModel
                {                    
                    Product = item.Product
                });
            }
            return View();
        }

        public IActionResult AddToCart(int id)
        {
            var product = _productRepository.Products.FirstOrDefault(p => p.Id == id);
            var cartItem = new ShoppingCartItem()
            {
                ShoppingCartItemModelId = id,
                Product = product,
                Amount = 1,
            };
            _shoppingCartItemRepository.ShoppingCartItems.Add(cartItem);

            return RedirectToAction("Index");

        }

    }
}


//public int RemoveFromCart(Product product)
//{
//    var cartItem = _itemRepository.ShoppingCartItems.SingleOrDefault(
//           c => c.Product.Id == product.Id
//           && c.ShoppingCartId == ShoppingCartId);
//    var localAmount = 0;
//    if (cartItem != null)
//    {
//        if (cartItem.Amount > 1)
//        {
//            cartItem.Amount--;
//            localAmount = cartItem.Amount;
//        }
//        else
//        {
//            _itemRepository.ShoppingCartItems.Remove(cartItem);
//        }
//    }
//    return localAmount;
//}
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