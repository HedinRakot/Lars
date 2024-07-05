namespace LarsProjekt.Domain.StoreApi;

public class Offer
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public List<Product> Products { get; } = [];
    public long TimePeriodId { get; set; }
    public List<TimePeriod> TimePeriods { get; } = [];
    public decimal? Price { get; set; }
}
