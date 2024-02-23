namespace MyTemsAPI.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task Delete(User user);
        Task<List<User>> GetAll();
        Task<User> GetById(long id);
        Task<User> GetByNameWithAddress(string name);
        Task<User> GetByName(string name);
        Task Update(User user);
    }
}