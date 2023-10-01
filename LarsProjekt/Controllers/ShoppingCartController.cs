using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IProductRepository _productRepository;
        private IShoppingCartItemRepository _cartRepository;
        public ShoppingCartController(IProductRepository productRepository, IShoppingCartItemRepository cartRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var list = new List<ShoppingCartItemModel>();
            foreach (var product in _cartRepository.GetAll())
            {
                list.Add(new ShoppingCartItemModel
                {
                    Id = product.Id,
                    Amount = product.Amount,
                    ShoppingCartId = product.ShoppingCartId
                });
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Add() 
        {
            return View(new ShoppingCartItemModel());
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var product = _productRepository.Get(id);
            
            var shoppingCartItem = _cartRepository.GetItem(id);
            if (shoppingCartItem == null)
            {
                var cartItem = new ShoppingCartItem
                {
                    Amount = 1,
                    Product = product
                };
                _cartRepository.AddProduct(cartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            return RedirectToAction(nameof(Index));

        }




        //        public IActionResult RemoveFromCart(int id)
        //        {
        //            var product = _productRepository.Get(id);
        //            _shoppingCartRepository.RemoveFromCart(product);
        //        //    var product = _shoppingCartRepository.ShoppingCartItems.FirstOrDefault(p => p.ItemId == id);
        //        //    if (product != null)
        //        //    {
        //        //        if (product.Amount > 1)
        //        //        {
        //        //            product.Amount--;

        //        //        }
        //        //        else
        //        //        {
        //        //            _shoppingCartRepository.ShoppingCartItems.Remove(product);
        //        //        }
        //        //    }
        //            return RedirectToAction(nameof(Index));
        //        }

    }
}