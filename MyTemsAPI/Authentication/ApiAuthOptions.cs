using MyTemsAPI.Domain;

namespace MyTemsAPI.Authentication;

public class ApiAuthOptions
{
    public const string Section = "Authentication";
    public List<AppUser> Users { get ; set; }
}
