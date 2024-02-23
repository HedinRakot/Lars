using MyTemsAPI.Domain.IRepositories;

namespace MyTemsAPI.Domain.IRepositories;

public interface ISqlUnitOfWork
{
    IOrderDetailRepository OrderDetailRepository { get; }
    IOrderRepository OrderRepository { get; }

    void SaveChanges();
    Task SaveChangesAsync();
}