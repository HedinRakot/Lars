using LarsProjekt.Domain.MyTemsApi;

namespace LarsProjekt.Domain.Interfaces;

public interface IUserService
{
    Task<User> Create(User user);
    Task Delete(long id);
    Task<List<User>> Get();
    Task<User> GetByNameWithAddress(string name);
    Task<User> GetByName(string name);
    Task<User> Update(User user);
}