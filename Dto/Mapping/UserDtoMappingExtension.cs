using LarsProjekt.Domain.MyTemsApi;
using LarsProjekt.Dto.MyTemsApi;

namespace LarsProjekt.Dto.Mapping;

public static class UserDtoMappingExtension
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Password,            
            user.Address?.ToDto(),
            user.AddressId
            );
    }

    public static User ToDomain(this UserDto dto)
    {
        return new User
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Password = dto.Password,
            AddressId = dto.AddressId
        };
    }
}
