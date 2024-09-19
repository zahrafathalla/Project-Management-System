

using global::ProjectManagementSystem.Data.Context;
using global::ProjectManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Repository.Interface;
using System.Linq.Expressions;

namespace ProjectManagementSystem.Repository.Repository
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        private readonly ApplicationDBContext _dBContext;

        public ProjectRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<IEnumerable<Project>> GetAllAsync(int skip = 0, int take = 10, string? searchTerm = null)
        {
            IQueryable<Project> query = _dBContext.Set<Project>().Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Title.Contains(searchTerm) || p.Description.Contains(searchTerm));
            }

            return await query.Skip(skip).Take(take).Include(p=>p.UserProjects).Include(p=>p.Tasks).ToListAsync();
        }
    }
}

