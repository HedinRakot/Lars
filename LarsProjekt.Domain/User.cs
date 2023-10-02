namespace LarsProjekt.Domain;

public class User
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<Order> Orders { get; set; }
}