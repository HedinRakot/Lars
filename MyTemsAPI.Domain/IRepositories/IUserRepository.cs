namespace MyTemsAPI.Domain.IRepositories
{
    public interface IUserRepository
    {
        void Add(User user);
        void Delete(User user);
        List<User> GetAll();
        User GetById(long id);
        User GetByNameWithAddress(string name);
        User GetByName(string name);
        void Update(User user);
    }
}