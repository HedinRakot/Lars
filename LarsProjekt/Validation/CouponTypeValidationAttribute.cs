using System.ComponentModel.DataAnnotations;

namespace LarsProjekt.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]

public class CouponTypeValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value != null && value.ToString() == "Percent" || value.ToString() == "Money")
            return true;

        return false;
    }
}
