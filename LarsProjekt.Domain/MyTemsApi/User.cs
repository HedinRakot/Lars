namespace LarsProjekt.Domain.MyTemsApi;

public class User
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Address? Address { get; set; }
    public long? AddressId { get; set; }
}