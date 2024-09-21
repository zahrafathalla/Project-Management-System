using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.Data.Configurations
{
    public class UserProjectConfig : IEntityTypeConfiguration<UserProject>
    {
        public void Configure(EntityTypeBuilder<UserProject> builder)
        {
            builder.Property(up=> up.Status)
                .HasConversion
                (
                    status => status.ToString(),
                    status => (InvitationStatus)Enum.Parse(typeof(InvitationStatus), status)
                );
        }
    }


}
