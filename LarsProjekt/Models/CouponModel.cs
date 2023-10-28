using System.ComponentModel.DataAnnotations;

namespace LarsProjekt.Models
{
    public class CouponModel
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Code { get; set; }
        public string? Discount { get; set; }
    }
}
