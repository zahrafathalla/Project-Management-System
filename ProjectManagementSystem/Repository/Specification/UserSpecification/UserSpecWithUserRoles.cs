using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.UserSpecification
{
    public class UserSpecWithUserRoles : BaseSpecification<User>
    {
        public UserSpecWithUserRoles(string email)
            :base(u => u.Email == email)
        {
            Includes.Add(u=>u.Include(u=>u.UserRoles).ThenInclude(u=>u.Role));
        }
    }
}
