using LarsProjekt.Domain;

namespace LarsProjekt.Application;

public interface IUserService
{
    Task<User> Create(User user);
    Task<string> Delete(long id);
    Task<List<User>> Get();
    Task<User> GetById(long id);
    Task<User> GetByName(string name);
    Task<User> Update(User user);
}