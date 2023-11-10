using Microsoft.AspNetCore.Mvc.Rendering;

namespace LarsProjekt.Models.ViewModels
{
    public class OrderConfirmationVM
    {
        public IEnumerable<ShoppingCartItemModel> Items { get; set; }
        public IEnumerable<OfferModel> Offers { get; set; }
        public CouponModel Coupons { get; set; }
        public string UserInput { get; set; }
        public decimal? Total {  get; set; }

    }
}
