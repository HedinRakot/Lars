using LarsProjekt.Domain;
using static LarsProjekt.Models.OrderModel;

namespace LarsProjekt.Models;

public class OrderDetailModel
{
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public int AlbumId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public virtual ProductModel Product { get; set; }
    public virtual OrderModel Order { get; set; }
}