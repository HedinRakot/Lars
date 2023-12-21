

namespace LarsProjekt.Domain
{
    public class Coupon
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string? Discount { get; set; }
        public string Type { get; set; } // Percent / Money
        public DateTimeOffset ExpiryDate { get; set; }
        public bool Expired { get; set; }
        public int Count { get; set; }
        public int AppliedCount { get; set; }
        public byte[] Version { get; private set; }

    }
}
