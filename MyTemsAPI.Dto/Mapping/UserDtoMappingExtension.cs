using MyTemsAPI.Domain;

namespace MyTemsAPI.Dto.Mapping;

public static class UserDtoMappingExtension
{
    public static User ToDomain(this UserDto dto)
    {
        return new User
        {
            Id = dto.Id,
            Username = dto.Username,
            Email = dto.Email,
            Password = dto.Password,
            Address = dto.Address.ToDomain(),
            AddressId = dto.AddressId
        };
    }
}
