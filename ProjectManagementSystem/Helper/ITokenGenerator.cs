using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Helper;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}
