using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarsProjekt.Domain;

public class ShoppingCartItem
{    
    public long ItemId { get; set; }    
    public int Amount { get; set; }
    public Product Product { get; set; }
    public string ShoppingCartId { get; set; }
}
