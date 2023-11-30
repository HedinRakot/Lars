using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace LarsProjekt.Models;

public class ProductModel
{
    [Required] public long Id { get; set; } = 0;

    [Required]
    [Display(Name = "Product Name")]
    [MaxLength(30)]
    public string Name { get; set; }

    [Required]
    [MinLength(20, ErrorMessage = "Please enter a detailed description")]
    [MaxLength(4000)]
    public string Description { get; set; }
    public string? Category { get; set; }

    [Required]
    [Range(1, 99999)]
    public decimal Price { get; set; }

    [Required]
    [Range(1, 99999)]
    public decimal PriceOffer { get; set; }

    [ValidateNever]
    public string? Image { get; set; }

}
