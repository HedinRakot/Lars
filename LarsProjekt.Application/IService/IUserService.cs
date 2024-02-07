using LarsProjekt.Domain;

namespace LarsProjekt.Application.IService;

public interface IUserService
{
    Task<User> Create(User user);
    Task Delete(long id);
    Task<List<User>> Get();
    Task<User> GetByNameWithAddress(string name);
    Task<User> GetByName(string name);
    Task<User> Update(User user);
}