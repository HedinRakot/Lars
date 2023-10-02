using LarsProjekt.Domain;
using static LarsProjekt.Models.OrderModel;

namespace LarsProjekt.Models;

public class OrderDetailModel
{
    public long Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public virtual ProductModel Product { get; set; }
    public virtual OrderModel Order { get; set; }
}