using MyTemsAPI.Domain;

namespace MyTemsAPI.Models.Mapping;

public static class UserModelMappingExtension
{
    public static UserModel ToModel(this User user)
    {
        return new UserModel
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Password = user.Password,
            AddressId = user.AddressId
        };
    }

    public static User ToDomain(this UserModel model)
    {
        return new User
        {
            Id = model.Id,
            Username = model.Username,
            Email = model.Email,
            Password = model.Password,
            AddressId = model.AddressId
        };
    }
}
