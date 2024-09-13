using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.User.Queries;
using ProjectManagementSystem.Errors;

namespace ProjectManagementSystem.CQRS.User.Commands;


public class LoginCommand : IRequest<Result<LoginResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IMediator _mediator;

    public LoginHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var authResult = await _mediator.Send(new GetTokenQuery { Email = request.Email, Password = request.Password });

        if (!authResult.IsSuccess)
            throw new UnauthorizedAccessException("Invalid credentials");
            //return Result.Failure(UserErrors.InvalidCredentials);


        var response = authResult.Data;
        var loginresponse =  new LoginResponse(response.Id, response.Email, response.Token, response.ExpiresIn);

        return Result.Success(loginresponse);
    }
}



public record LoginResponse(
    int Id,
    string? Email,
    string Token,
    int ExpiresIn
    );


