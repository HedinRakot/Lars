using LarsProjekt.Domain;

namespace LarsProjekt.Application;

public class UserRepository
{
    public UserRepository()
    {
        Users = new List<User>();
        Users.Add(new User()
        {
            Id = 1,
            Name = "admin"
        });
    }

    public List<User> Users { get; set; }
}
