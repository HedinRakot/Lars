

namespace MyTemsAPI.Dto;

public record UserDto(
    long Id ,
    string Username,
    string Email,
    string Password,
    AddressDto Address,
    long AddressId
    );


