using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Services;

public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(User user);
}
