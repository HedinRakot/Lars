namespace LarsProjekt.Models
{
    public class OfferModel
    {
        public long Id { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string DiscountValue { get; set; }
        public decimal Discount { get; set; }
        public string CouponCode { get; set; }
        

    }
}
