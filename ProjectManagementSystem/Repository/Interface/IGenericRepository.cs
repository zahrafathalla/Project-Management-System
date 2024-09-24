using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Specification;
using System.Linq.Expressions;

namespace ProjectManagementSystem.Repository.Interface
{
    public interface IGenericRepository<T> where T: BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(int skip , int take);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> GetWithInclude(Expression<Func<T, bool>> expression);
        Task<T?> GetByIdAsync(int id);
        Task<int> GetCountAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteById(int id);
        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> Spec);
        Task<T?> GetByIdWithSpecAsync(ISpecification<T> Spec);
        Task<int> GetCountWithSpecAsync(ISpecification<T> Spec);

    }
}
