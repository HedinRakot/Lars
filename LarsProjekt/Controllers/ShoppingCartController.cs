using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
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
            cart.Discount = CalcDiscount() + CalcMoneyDiscount();

            foreach (var item in cart.Offers)
            {
                decimal.TryParse(item.Coupon.Discount, out decimal x);

                if( item.Coupon.Type == "Percent")
                {
                    item.Discount = (x / 100) * GetTotal();
                }
                else
                {
                    item.Discount = x;
                }                
            }

            foreach (var cartItem in cart.Items)
            {
                cartItem.DiscountedPrice = CalcDiscountedItemPrice(cartItem.PriceOffer, cartItem.Amount);
                cartItem.Discount = CalcItemDiscount(cartItem.PriceOffer);
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
            coupon.Expired = Expired(coupon);
            _couponRepository.Update(coupon);

            if (coupon.Expired == false) 
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
                    cart.Discount = CalcDiscount() + CalcMoneyDiscount();

                    if (cart.Discount == 0)
                    {
                        if (coupon.Type == "Percent")
                        {
                            cart.Discount = (x / 100) * cart.Total;
                            cart.Total = cart.Total - cart.Discount;
                        }
                        else
                        {
                            cart.Discount = x;
                        }
                    }

                    var model = new OfferModel
                    {
                        Id = cart.Offers.Count > 0 ? cart.Offers.Max(o => o.Id) + 1 : 1,
                        Coupon = coupon.ToModel()
                    };

                    if (coupon.Type == "Percent")
                    {
                        model.Discount = (x / 100) * GetTotal();
                    }
                    else
                    {
                        model.Discount = x;
                    }

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
                    cart.Discount = CalcDiscount() + CalcMoneyDiscount();
                    string message = "This code can only be used once for an order.";
                    return Json(new { success = "false", message, total = cart.Total });
                }
            }
            else
            {
                cart.Total = CalcDiscountedTotal();
                string message = "This code has expired.";
                return Json(new { success = "false", message, total = cart.Total });
            }
        }
        else
        {
            cart.Total = CalcDiscountedTotal();
            string message = "This code does not exist.";
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

    private decimal CalcDiscountedItemPrice(decimal price, int amount)
    {        
        var moneyDiscount = CalcCartItemMoneyDiscount();
        var discount = CalcCartItemDiscount(price);
        price = (price - discount) * amount - moneyDiscount;
        return price;
    }

    private decimal CalcItemDiscount(decimal price)
    {
        var moneyDiscount = CalcCartItemMoneyDiscount();
        var discount = CalcCartItemDiscount(price);
        return moneyDiscount + discount;
    }

    private decimal CalcCartItemDiscount(decimal price)
    {
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cart = new CartModel();
        var cookieValue = Request.Cookies[cookie];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            cart = JsonSerializer.Deserialize<CartModel>(cookieValue);
        }        
        var list = new List<string>();

        foreach (var item in cart.Offers)
        {
            if (item.Coupon.Type == "Percent")
            {
                list.Add(item.Coupon.Discount);
            }
        }
        var value = list.Sum(x => Convert.ToDecimal(x));
        return (value / 100) * price;

    }

    private decimal CalcDiscountedTotal()
    {        
        var total = GetTotal();
        var moneyDiscount = CalcMoneyDiscount();
        var discount = CalcDiscount();
        total = total - moneyDiscount - discount;

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
        var list = new List<string>();

        foreach (var item in cart.Offers)
        {            
            if (item.Coupon.Type == "Percent")
            {
                list.Add(item.Coupon.Discount);
            }            
        }
        var value = list.Sum(x => Convert.ToDecimal(x));
        return (value / 100) * total;
    }

    private decimal CalcCartItemMoneyDiscount()
    {
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cart = new CartModel();
        var cookieValue = Request.Cookies[cookie];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            cart = JsonSerializer.Deserialize<CartModel>(cookieValue);
        }

        var list = new List<string>();

        foreach (var item in cart.Offers)
        {
            if (item.Coupon.Type == "Money")
            {
                list.Add(item.Coupon.Discount);
            }

        }
        var value = list.Sum(x => Convert.ToDecimal(x));

        return value / cart.Items.Count;
    }

    private decimal CalcMoneyDiscount()
    {
        var user = HttpContext.User.Identity.Name;
        var cookie = $"shoppingCart{user}";
        var cart = new CartModel();
        var cookieValue = Request.Cookies[cookie];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            cart = JsonSerializer.Deserialize<CartModel>(cookieValue);
        }

        var list = new List<string>();

        foreach ( var item in cart.Offers )
        {            
            if(item.Coupon.Type == "Money")
            {
                list.Add(item.Coupon.Discount);
            }
            
        }
        var value = list.Sum(x => Convert.ToDecimal(x));
        return value;

    }

    private bool Expired(Coupon coupon)
    {
        if (coupon == null)
        {
            return false;
        }
        if (coupon.ExpiryDate < DateTimeOffset.Now || coupon.Count <= coupon.AppliedCount)
        {
            return true;
        }
        return false;
    }

}