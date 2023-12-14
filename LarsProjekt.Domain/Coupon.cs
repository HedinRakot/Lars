

namespace LarsProjekt.Domain
{
    public class Coupon
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string? Discount { get; set; }
        public string Type { get; set; } // Percent / Money / BuyXGetY
    }
}
