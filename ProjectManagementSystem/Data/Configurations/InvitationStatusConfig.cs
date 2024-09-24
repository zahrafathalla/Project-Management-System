using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.Data.Configurations;

public class InvitationStatusConfig : IEntityTypeConfiguration<WorkTask>
{
    public void Configure(EntityTypeBuilder<WorkTask> builder)
    {
        builder.Property(p => p.Priority)
            .HasConversion
            (
                status => status.ToString(),
                status => (TaskPriority)Enum.Parse(typeof(TaskPriority), status)
            );
    }
}
