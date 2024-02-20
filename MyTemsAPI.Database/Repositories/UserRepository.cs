using MyTemsAPI.Database;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Domain;

namespace MyTemsAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<User> GetAll()
    {
        return _context.Users.ToList();
    }

    public User GetById(long id)
    {
        return _context.Users.FirstOrDefault(x => x.Id == id);
    }
    public User GetByName(string name)
    {
        User user = _context.Users.FirstOrDefault(x => x.Username == name);        
        return user;
    }
    public User GetByNameWithAddress(string name)
    {
        User user = _context.Users.FirstOrDefault(x => x.Username == name);
        user.Address = _context.Address.FirstOrDefault(x => x.Id == user.AddressId);
        return user;
    }
    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
    public void Delete(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}
