using Microsoft.EntityFrameworkCore;
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


    }
}
