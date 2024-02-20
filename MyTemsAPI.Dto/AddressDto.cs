namespace MyTemsAPI.Dto;

public record AddressDto(
    long Id,
    string FirstName,
    string LastName,
    string Street,
    string HouseNumber,
    string City,
    string State,
    string PostalCode,
    string Country,
    string Phone
);

