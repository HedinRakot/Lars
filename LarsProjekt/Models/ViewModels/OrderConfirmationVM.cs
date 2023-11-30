
namespace LarsProjekt.Models.ViewModels
{
    public class OrderConfirmationVM
    {
        public CartModel Cart { get; set; }

        //public IEnumerable<ShoppingCartItemModel> Items { get; set; }
        public IEnumerable<OfferModel> Offers { get; set; }
        public CouponModel Coupons { get; set; }
        public string UserInput { get; set; }
        public decimal? Total {  get; set; }

    }
}
