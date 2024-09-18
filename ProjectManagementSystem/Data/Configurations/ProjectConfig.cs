using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.Data.Configurations
{
    public class ProjectConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(p => p.Status)
                .HasConversion
                (
                    status => status.ToString(),
                    status => (ProjectStatus)Enum.Parse(typeof(ProjectStatus), status)
                );
        }
    }
}
