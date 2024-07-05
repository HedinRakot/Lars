using LarsProjekt.Domain.MyTemsApi;

namespace LarsProjekt.Models.Mapping;

public static class UserModelMappingExtension
{
    public static UserModel ToModel(this User user)
    {
        return new UserModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password
        };
    }

    public static User ToDomain(this UserModel model)
    {
        return new User
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password
        };
    }
}
