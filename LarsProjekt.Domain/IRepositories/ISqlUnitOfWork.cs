using LarsProjekt.Database.Repositories;

namespace LarsProjekt.Database
{
    public interface ISqlUnitOfWork
    {
        IAddressRepository AddressRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IOrderRepository OrderRepository { get; }
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }

        void SaveChanges();
    }
}