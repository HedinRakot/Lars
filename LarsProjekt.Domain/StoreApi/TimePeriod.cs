namespace LarsProjekt.Domain.StoreApi;

public class TimePeriod
{
    public long Id { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public List<Product> Products { get; } = [];
}
