using LarsProjekt.Domain;

namespace LarsProjekt.Application
{
    public class DiscountPriceRepository
    {
        public DiscountPrice Price { get; set; }
        public DiscountPriceRepository() 
        {
            Price = new DiscountPrice();
        }
    }
}
