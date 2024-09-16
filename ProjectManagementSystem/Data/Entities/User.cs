namespace ProjectManagementSystem.Data.Entities
{
    public class User: BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 
        public bool IsEmailVerified { get; set; } 
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string? VerificationToken { get; set; }
        public string? PasswordResetCode { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
