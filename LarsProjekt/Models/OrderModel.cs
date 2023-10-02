using LarsProjekt.Domain;

namespace LarsProjekt.Models;

public partial class OrderModel
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
    public decimal? Total { get; set; } = 0;
    public DateTimeOffset Date { get; set; }
    public UserModel User { get; set; }
    public long UserId { get; set; }
    public string? OrderNumber { get; set; }
}
