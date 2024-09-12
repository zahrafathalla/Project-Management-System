using MediatR;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.CQRS.User.Commands;


public class LoginCommand : IRequest<LoginResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserService _userService;

    public LoginHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetTokenAsync(request.Email, request.Password);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        return new LoginResponse(user.Value.Id, user.Value.Email, user.Value.Token, user.Value.ExpiresIn);
    }
}


public record LoginResponse(
    int Id,
    string? Email,
    string Token,
    int ExpiresIn
    );


