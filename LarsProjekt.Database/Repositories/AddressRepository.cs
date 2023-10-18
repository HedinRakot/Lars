using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

internal class AddressRepository : IAddressRepository
{
	private readonly ApplicationDbContext _context;

	public AddressRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public List<Address> GetAll()
	{
		return _context.Address.ToList();
	}
	public void Add(Address address)
	{
		_context.Address.Add(address);
		_context.SaveChanges();
	}

	public Address Get(long id)
	{
		return _context.Address.FirstOrDefault(u => u.Id == id);
	}

	public void Update(Address address)
	{
		_context.Address.Update(address);
		_context.SaveChanges();
	}

	public void Delete(Address address)
	{
		_context.Address.Remove(address);
		_context.SaveChanges();
	}
}
