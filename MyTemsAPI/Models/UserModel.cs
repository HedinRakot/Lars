using System.ComponentModel.DataAnnotations;

namespace MyTemsAPI.Models;

public class UserModel
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public long AddressId { get; set; }
}
