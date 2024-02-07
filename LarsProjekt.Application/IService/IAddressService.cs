using LarsProjekt.Domain;

namespace LarsProjekt.Application.IService;

public interface IAddressService
{
    Task<Address> Create(Address address);
    Task Delete(long id);
    Task<Address> GetById(long id);
    Task<List<Address>> Get();
    Task<Address> Update(Address address);
}