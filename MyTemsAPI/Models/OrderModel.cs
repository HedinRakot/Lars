namespace MyTemsAPI.Models;

public class OrderModel
{
    public long Id { get; set; }
    public decimal? Total { get; set; }
    public DateTimeOffset Date { get; set; }
}
