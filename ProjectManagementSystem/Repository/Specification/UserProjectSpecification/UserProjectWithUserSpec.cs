using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.UserProjectSpecification
{
    public class UserProjectWithUserSpec :BaseSpecification<UserProject>
    {
        public UserProjectWithUserSpec(int userId)
            :base (up => up.UserId == userId)
        {
            Includes.Add(up => up.Include(up => up.User));
            Includes.Add(up => up.Include(up => up.Project));

        }
        public UserProjectWithUserSpec(IEnumerable<int> projectIds)
            :base(up => projectIds.Contains(up.ProjectId))
        {
            Includes.Add(up => up.Include(up => up.User));
            Includes.Add(up => up.Include(up => up.Project));


        }
    }
}
