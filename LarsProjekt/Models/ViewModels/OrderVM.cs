using LarsProjekt.Domain;

namespace LarsProjekt.Models.ViewModels
{
    public class OrderVM
    {
        public IEnumerable<OrderDetailModel> Details { get; set; }

    }
}
