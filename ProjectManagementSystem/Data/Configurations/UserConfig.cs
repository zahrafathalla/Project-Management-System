using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.Data.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Status)
                .HasConversion
                (
                    status => status.ToString(),
                    status => (UserStatus) Enum.Parse(typeof(UserStatus), status)
                );

        }
    }
}
