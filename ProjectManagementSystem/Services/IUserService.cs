using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Contract;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Services;

public interface IUserService
{
    Task<User> AuthenticateAsync(string username, string password);
    string GenerateJwtToken(User user);
    Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);

}