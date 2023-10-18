namespace LarsProjekt.Domain;

public class Order
{
    public long Id { get; set; }
    public decimal? Total { get; set; }
    public DateTimeOffset Date { get; set; }
    public User User { get; set; }
    public long UserId { get; set; }
    public Address Address { get; set; }
    public long AddressId { get; set; }
}