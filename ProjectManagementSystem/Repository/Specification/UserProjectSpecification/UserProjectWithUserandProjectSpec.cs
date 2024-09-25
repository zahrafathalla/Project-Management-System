using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.UserProjectSpecification
{
    public class UserProjectWithUserandProjectSpec :BaseSpecification<UserProject>
    {
        public UserProjectWithUserandProjectSpec(int userId)
            :base (up => up.UserId == userId)
        {
            Includes.Add(up => up.Include(up => up.User));
            Includes.Add(up => up.Include(up => up.Project));

        }
        public UserProjectWithUserandProjectSpec(IEnumerable<int> projectIds)
            :base(up => projectIds.Contains(up.ProjectId))
        {
            Includes.Add(up => up.Include(up => up.User));
            Includes.Add(up => up.Include(up => up.Project));


        }
    }
}
