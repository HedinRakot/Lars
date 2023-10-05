using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        User Get(long id);
        List<User> GetAll();
        void Update(User user);
        void Delete (User user);
        public User GetByName(string name);
    }
}