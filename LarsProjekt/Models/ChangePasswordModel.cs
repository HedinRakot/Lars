using System.ComponentModel.DataAnnotations;

namespace LarsProjekt.Models
{
    public class ChangePasswordModel 
    {
        public long Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordRepeat { get; set; }
    }
}
