using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.ProjectSpecifications
{
    public class ProjectWithUserProjectSpec :BaseSpecification<Project>
    {
        public ProjectWithUserProjectSpec(int userId)
            :base (p=> p.UserProjects.Any(up=> up.UserId == userId))
        {
            Includes.Add(p=>p.Include(p=>p.UserProjects));
        }
    }
}
