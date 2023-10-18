using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

public interface IAddressRepository
{
	void Add(Address address);
	void Delete(Address address);
	Address Get(long id);
	List<Address> GetAll();
	void Update(Address address);
}