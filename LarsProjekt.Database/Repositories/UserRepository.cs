using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<User> GetAll()
    {
        var users = _context.Users;
        return users.ToList();
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User Get(long id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public User GetByName(string name)
    {
        return _context.Users.FirstOrDefault(u => u.Username == name);
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
