using System.ComponentModel.DataAnnotations;

namespace LarsProjekt.Models;

public class CartModel
{
    [Key]
    public int RecordId { get; set; }
    public string CartId { get; set; }
    public int ProductId { get; set; }
    public int Count { get; set; }
    public DateTime DateCreated { get; set; }
    public ProductModel Product { get; set; }
}
