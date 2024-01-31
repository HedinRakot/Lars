using LarsProjekt.Domain;

namespace LarsProjekt.Dto.Mapping;

public static class AddressDtoMappingExtension
{
    public static AddressDto ToDto(this Address address)
    {
        return new AddressDto(
            address.Id,
            address.FirstName,
            address.LastName,
            address.Street,
            address.HouseNumber,
            address.City,
            address.State,
            address.PostalCode,
            address.Country,
            address.Phone
            );
    }
    public static Address ToDomain(this AddressDto dto)
    {
        return new Address
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Street = dto.Street,
            HouseNumber = dto.HouseNumber,
            City = dto.City,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            Phone = dto.Phone,
            State = dto.State
        };
    }
}
