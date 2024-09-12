using Microsoft.AspNetCore.Http.HttpResults;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Contract;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Repository;
using System.Linq.Expressions;

namespace ProjectManagementSystem.Services;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _repository;
    private readonly IJwtProvider _jwtProvider;

    public UserService(GenericRepository<User> repository,IJwtProvider jwtProvider)
    {
        _repository = repository;
        _jwtProvider = jwtProvider;
    }
    public Task<User> AuthenticateAsync(string username, string password)
    {
        // Authenticate user logic here (e.g., check against a database)

        User user = new User();
        return Task.FromResult(user);
    }

    public string GenerateJwtToken(User user)
    {
        // Token generation logic here

        return "";
    }
    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {

        var users = await _repository.GetAsync(u => u.Email == email && !u.IsDeleted);
        var user = users.FirstOrDefault();

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var isValidPassword = true; // Waut Meyhod to compare between hashPassword and stordPassword

        if (!isValidPassword)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);

        var response = new AuthResponse(user.Id, user.Email, user.UserName, user.UserName, token, expiresIn);

        return Result.Success(response);
    }
}
