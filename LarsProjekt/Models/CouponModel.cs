using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        [Required]
        [Validation.CouponTypeValidation(ErrorMessage = "Please enter Money or Percent.")]
        public string Type { get; set; }
        
        [Required]
        public DateTimeOffset ExpiryDate { get; set; }
        [Required]
        public bool Expired { get; set; }
        public int Count { get; set; }
        public int AppliedCount { get; set; }
        [ValidateNever]
        public byte[] Version { get; private set; }

    }
}
