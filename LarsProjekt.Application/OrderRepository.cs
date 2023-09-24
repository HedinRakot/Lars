
using LarsProjekt.Domain;

namespace LarsProjekt.Application;

public class OrderRepository
{
    public List<Order> Orders { get; set; } = new List<Order>();
    public List<OrderDetail> Details { get; set; } = new List<OrderDetail>();
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
}
