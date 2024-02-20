using MyTemsAPI.Domain;

namespace MyTemsAPI.Dto.Mapping;

public static class AddressDtoMappingExtension
{
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
