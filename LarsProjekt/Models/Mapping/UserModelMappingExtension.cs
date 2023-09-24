using LarsProjekt.Domain;

namespace LarsProjekt.Models.Mapping;

public static class UserModelMappingExtension
{
    public static UserModel ToModel(this User user)
    {
        return new UserModel
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Description = user.Description,
            Email = user.Email,
            Number = user.Number
        };
    }

    public static User ToDomain(this UserModel model)
    {
        return new User
        {
            Id = model.Id,
            Name = model.Name,
            LastName = model.LastName,
            Description = model.Description,
            Email = model.Email,
            Number = model.Number
        };
    }
}
