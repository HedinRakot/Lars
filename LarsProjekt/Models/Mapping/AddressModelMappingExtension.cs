using LarsProjekt.Domain;

namespace LarsProjekt.Models.Mapping;

public static class AddressModelMappingExtension
{
    public static AddressModel ToModel(this Address address)
    {
        return new AddressModel
		{
            Id = address.Id,
            FirstName = address.FirstName,
            LastName = address.LastName,
            Street = address.Street,
            City = address.City,
            State = address.State,
            PostalCode = address.PostalCode,
            Country = address.Country,
            HouseNumber = address.HouseNumber,
            Phone = address.Phone
        };
    }

    public static Address ToDomain(this AddressModel model)
    {
        return new Address
		{
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Street = model.Street,
            City = model.City,
            State = model.State,
            PostalCode = model.PostalCode,
            Country = model.Country,
            HouseNumber = model.HouseNumber,
            Phone = model.Phone
        };
    }
}
