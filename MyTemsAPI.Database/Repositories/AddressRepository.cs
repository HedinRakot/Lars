using MyTemsAPI.Database;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Domain;
using Microsoft.EntityFrameworkCore;


namespace MyTemsAPI.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly AppDbContext _context;
    public AddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Address>> GetAll() => await _context.Address.ToListAsync();

    public async Task<Address> GetById(long id) => await _context.Address.FirstOrDefaultAsync(x=>x.Id==id);    

    public async Task Add(Address address)
    {
        _context.Address.Add(address);
        await _context.SaveChangesAsync();
    }
    public async Task Update(Address address)
    {
        _context.Address.Update(address);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(Address address)
    {
        _context.Address.Remove(address);
        await _context.SaveChangesAsync();
    }
}
