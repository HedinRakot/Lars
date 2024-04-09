namespace LarsProjekt.Messages.Dtos;

public class CouponDto
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string? Discount { get; set; }
    public string Type { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }
    public bool Expired { get; set; }
    public int Count { get; set; }
    public int AppliedCount { get; set; }
    public string? Version { get; set; }  // byte[] to string mapping

}
