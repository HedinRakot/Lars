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
        [ValidateNever]
        //[Validation.CouponTypeValidation(ErrorMessage = "Please choose one of the given values")]
        public string Type { get; set; }
    }
}
