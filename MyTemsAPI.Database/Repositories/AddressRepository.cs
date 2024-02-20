using MyTemsAPI.Database;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Domain;


namespace MyTemsAPI.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly AppDbContext _context;
    public AddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Address> GetAll()
    {
        return _context.Address.ToList();
    }

    public Address GetById(long id)
    {
        return _context.Address.FirstOrDefault(x => x.Id == id);
    }

    public void Add(Address address)
    {
        _context.Address.Add(address);
        _context.SaveChanges();
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
