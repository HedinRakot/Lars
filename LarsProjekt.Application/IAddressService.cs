using LarsProjekt.Domain;

namespace LarsProjekt.Application;

public interface IAddressService
{
    Task<Address> Create(Address address);
    Task<string> Delete(long id);
    Task<Address> GetById(long id);
    Task<List<Address>> Get();
    Task<Address> Update(Address address);
}