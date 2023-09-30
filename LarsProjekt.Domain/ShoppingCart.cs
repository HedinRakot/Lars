using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LarsProjekt.Domain;
public class ShoppingCart
{
    public string ShoppingCartId { get; set; }
    public List<ShoppingCartItem> Items { get; set; }

}
