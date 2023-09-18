using LarsProjekt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LarsProjekt.Application;

public class ShoppingCartRepository
{
    public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    public List<ShoppingCart> ShoppingCarts { get; set; }
    public ShoppingCartRepository()
    {
        ShoppingCarts = new List<ShoppingCart>();
        ShoppingCartItems = new List<ShoppingCartItem>();
    }
}
