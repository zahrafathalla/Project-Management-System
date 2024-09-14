using MediatR;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Helper;

namespace ProjectManagementSystem.CQRS.Users.Commands;


public class LoginCommand : IRequest<LoginResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
public class LoginResponse()
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public bool IsSuccessed { get; set; } = true;
}

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IMediator _mediator;

    public LoginHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        //var authResult = await _mediator.Send(new GetTokenQuery { Email = request.Email, Password = request.Password });

        //if (!authResult.IsSuccess)
        //    throw new UnauthorizedAccessException("Invalid credentials");
        //    //return Result.Failure(UserErrors.InvalidCredentials);

        var result = new LoginResponse
        {
            IsSuccessed = false,
        };

        var user = await _mediator.Send(new GetUserByEmailQuery { Email = request.Email });

        if (user == null || !PasswordHasher.checkPassword(request.Password, user.PasswordHash) || !user.IsEmailVerified)
            return result;

        var loginResponse =  user.Map<LoginResponse>();
        loginResponse.Token = await _mediator.Send(new GenerateTokenCommand { User = user });

        return loginResponse;
    }
}





