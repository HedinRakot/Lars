using LarsProjekt.Application;
using LarsProjekt.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LarsProjekt.Models;

public partial class ShoppingCartModel
{
    [Key]
    public long Id { get; set; }

    public int ProductId { get; set; }
    [ValidateNever]
    [ForeignKey(nameof(ProductId))]
    public ProductModel Product { get; set; }

    public int Count { get; set; }

    public string UserId { get; set; }
    [ValidateNever]
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    
}