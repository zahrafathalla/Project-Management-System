using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.Data.Configurations
{
    public class TaskConfig : IEntityTypeConfiguration<WorkTask>
    {
        public void Configure(EntityTypeBuilder<WorkTask> builder)
        {
            builder.Property(t => t.Status)
                .HasConversion
                (
                    status => status.ToString(),
                    status => (WorkTaskStatus)Enum.Parse(typeof(WorkTaskStatus), status)
                );

            builder.Property(t => t.Priority)
                .HasConversion
                (
                    status => status.ToString(),
                    status => (TaskPriority)Enum.Parse(typeof(TaskPriority), status)
                );
        }
    }
}
