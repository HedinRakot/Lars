

namespace MyTemsAPI.Domain.IRepositories
{
    public interface IAddressRepository
    {
        Task Add(Address address);
        Task Delete(Address address);
        Task<List<Address>> GetAll();
        Task<Address> GetById(long id);
        Task Update(Address address);
    }
}