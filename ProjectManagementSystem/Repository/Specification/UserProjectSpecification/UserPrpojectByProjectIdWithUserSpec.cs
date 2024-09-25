using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.UserProjectSpecification
{
    public class UserPrpojectByProjectIdWithUserSpec :BaseSpecification<UserProject>
    {
        public UserPrpojectByProjectIdWithUserSpec(int projectId)
                 : base(p => p.ProjectId == projectId)
        {
            Includes.Add(p => p.Include(p => p.User));
        }
    }
}
