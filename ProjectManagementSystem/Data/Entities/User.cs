using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.Data.Entities
{
    public class User: BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 
        public bool IsEmailVerified { get; set; } =false;
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
        public string? VerificationToken { get; set; }
        public string? PasswordResetCode { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
    }
}
