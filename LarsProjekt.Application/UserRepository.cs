using LarsProjekt.Domain;

namespace LarsProjekt.Application;

public class UserRepository
{
    public UserRepository()
    {
        Users = new List<User>
        {
            new User()
            {
                Id = 1,
                Name = "Lars",
                LastName = "Ludwig",
                Email = "email@test.de",
                Description = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr."                

            },
            new User()
            {
                Id = 2,
                Name = "Peter",
                LastName ="Lustig",
                Email = "peter@test.de",
                Description = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua."
            }
        };
    }

    public List<User> Users { get; set; }
}
