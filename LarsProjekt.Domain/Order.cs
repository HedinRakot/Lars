namespace LarsProjekt.Domain;

public class Order
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public decimal? Total { get; set; }
    public DateTimeOffset Date { get; set; }
    public User User { get; set; }
    public long UserId { get; set; }
}