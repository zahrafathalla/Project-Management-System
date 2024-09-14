using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;
using System.Reflection;

namespace ProjectManagementSystem.Data.Context
{
    public class ApplicationDBContext :DbContext 
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            :base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

    }
}
