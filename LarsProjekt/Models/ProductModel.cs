using System.ComponentModel.DataAnnotations;

namespace LarsProjekt.Models;

public class ProductModel
{
    [Required] public long Id { get; set; } = 0;
    [Required] public string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    [Required] public int Price { get; set; }

}
