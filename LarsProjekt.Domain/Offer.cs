namespace LarsProjekt.Domain
{
    public class Offer
    {
        public long Id { get; set; }
        public decimal Discount { get; set; }
        public Coupon Coupon { get; set; } = new Coupon();

    }
}
