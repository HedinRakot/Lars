using LarsProjekt.Domain;
using System.ComponentModel.DataAnnotations;

namespace LarsProjekt.Models;

public class ShoppingCartItemModel
{
    [Key]
    public long Id { get; set; }
    public ProductModel Product { get; set; }
    public int Amount { get; set; }
    public long ProductId { get; set; }   
    public string ShoppingCartId { get; set; }

}
