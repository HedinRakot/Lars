namespace LarsProjekt.Models
{
    public class OfferModel
    {
        public long Id { get; set; }
        public decimal Discount { get; set; }
        public CouponModel Coupon { get; set; } = new CouponModel();

    }
}
