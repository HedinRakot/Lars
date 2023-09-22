using System.ComponentModel.DataAnnotations;

namespace LarsProjekt.Models;

public class UserModel
{
    public long Id { get; set; } = 0;

    [Required]
    [StringLength(10, ErrorMessage = "Name length should be between 3 and 10 symbols", MinimumLength = 3)]
    public string Name { get; set; }
    [Required]
    public string LastName { get; set; }
    public string? Description { get; set; }

    [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,})+)$",
        ErrorMessage = "Please enter a valid email adress")]
    public string? Email { get; set; }

    [Range(1, 5)]
    public int Number { get; set; }
}
