using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> SaveChangesAsync();

    }
}
