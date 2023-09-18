using LarsProjekt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarsProjekt.Application;

public class ShoppingCartItemRepository
{
    public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    public ShoppingCartItemRepository()
    {
        ShoppingCartItems = new List<ShoppingCartItem>
        {
            new ShoppingCartItem()
            {

            }
        };
    }

}
