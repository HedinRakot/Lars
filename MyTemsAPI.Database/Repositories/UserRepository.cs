using MyTemsAPI.Database;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace MyTemsAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAll() => await _context.Users.ToListAsync();
    public async Task<User> GetById(long id) => await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    public async Task<User> GetByName(string name)
    {
        User user = await _context.Users.FirstOrDefaultAsync(x => x.Username == name);        
        return user;
    }
    public async Task<User> GetByNameWithAddress(string name)
    {
        User user = await _context.Users.FirstOrDefaultAsync(x => x.Username == name);
        user.Address = await _context.Address.FirstOrDefaultAsync(x => x.Id == user.AddressId);
        return user;
    }
    public async Task Add(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
    public async Task Update(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
