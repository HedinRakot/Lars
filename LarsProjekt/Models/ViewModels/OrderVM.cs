using LarsProjekt.Domain;

namespace LarsProjekt.Models.ViewModels
{
    public class OrderVM
    {
        public IEnumerable<OrderDetailModel> Details { get; set; }
        public decimal Discount { get; set; }    
        public decimal Total { get; set; }
        public List<OfferModel> Offers { get; set; }
    }
}
