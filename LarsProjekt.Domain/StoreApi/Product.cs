namespace LarsProjekt.Domain.StoreApi;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    //public long CategoryId { get; set; }
    public List<Category> Categories { get; } = [];
    public List<TimePeriod> TimePeriods { get; } = [];
    public string? Image { get; set; }
}
