using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Interface;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync(int skip = 0, int take = 10, string? searchTerm = null);
}
