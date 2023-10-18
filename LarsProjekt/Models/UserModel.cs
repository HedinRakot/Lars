using System.ComponentModel.DataAnnotations;

namespace LarsProjekt.Models;

public class UserModel
{
    [Key]
    public long Id { get; set; }

    [Required]
    [StringLength(20, ErrorMessage = "Name length should be between 3 and 20 symbols", MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,})+)$",
        ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

}
