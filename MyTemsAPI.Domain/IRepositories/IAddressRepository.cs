

namespace MyTemsAPI.Domain.IRepositories
{
    public interface IAddressRepository
    {
        void Add(Address address);
        void Delete(Address address);
        List<Address> GetAll();
        Address GetById(long id);
        void Update(Address address);
    }
}