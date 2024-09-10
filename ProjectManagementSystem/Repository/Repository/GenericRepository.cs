using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Context;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using System.Linq.Expressions;

namespace ProjectManagementSystem.Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDBContext _dBContext;

        public GenericRepository(ApplicationDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dBContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression)
        {
            var query = _dBContext.Set<T>().AsQueryable();
            query = query.Where(x => !x.IsDeleted);
            query = query.Where(expression);
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dBContext.Set<T>().Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(T entity)
        {
            await _dBContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dBContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }

    }
}
