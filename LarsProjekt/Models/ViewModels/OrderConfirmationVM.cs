
namespace LarsProjekt.Models.ViewModels
{
    public class OrderConfirmationVM
    {
        public IEnumerable<ShoppingCartItemModel> Items { get; set; }
        public CouponModel Coupons { get; set; }
        public string UserInput { get; set; }
        public decimal? Total {  get; set; }
        //public decimal PriceAfterDiscount { get; set;}
    }
}
