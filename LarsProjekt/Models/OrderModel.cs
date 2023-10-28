using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LarsProjekt.Models;

public partial class OrderModel
{
    public long Id { get; set; }
    public decimal? Total { get; set; }
    public DateTimeOffset Date { get; set; }

    [ValidateNever]
    public AddressModel Address { get; set; }
    
}
