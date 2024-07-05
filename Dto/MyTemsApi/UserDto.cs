namespace LarsProjekt.Dto.MyTemsApi;

public record UserDto(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    AddressDto? Address,
    long? AddressId
    );


