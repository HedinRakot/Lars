using System.ComponentModel.DataAnnotations;

namespace LarsProjekt.Models;

public class UserModel
{
    public long Id { get; set; } = 0;

    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Email { get; set; }
}
