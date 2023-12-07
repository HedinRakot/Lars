namespace LarsProjekt.Models
{
    public class OfferModel
    {
        public long Id { get; set; }
        public decimal DiscountedPrice { get; set; }
        public CouponModel Coupon { get; set; } = new CouponModel();

    }
}
