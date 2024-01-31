using LarsProjekt.Domain;

namespace LarsProjekt.Dto.Mapping;

public static class UserDtoMappingExtension
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.Username,
            user.Password,
            user.Email
            );
    }

    public static User ToDomain(this UserDto dto)
    {
        return new User
        {
            Id = dto.Id,
            Username = dto.Username,
            Email = dto.Email,
            Password = dto.Password
        };
    }
}
