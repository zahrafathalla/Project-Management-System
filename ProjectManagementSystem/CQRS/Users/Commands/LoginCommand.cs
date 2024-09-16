using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;

namespace ProjectManagementSystem.CQRS.Users.Commands
{
    public record LoginCommand (string Email, string Password) : IRequest<Result<LoginResponse>>;
    
    public record LoginResponse(int Id, string Email, string Token);

    public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IMediator _mediator;
        private readonly ITokenGenerator _tokenGenerator;

        public LoginHandler(IMediator mediator, ITokenGenerator tokenGenerator)
        {
            _mediator = mediator;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
            }

            var userResult = await _mediator.Send(new GetUserByEmailQuery(request.Email));

            if (!userResult.IsSuccess)
            {
                return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
            }

            var user = userResult.Data;
            if (!PasswordHasher.checkPassword(request.Password, user.PasswordHash) || !user.IsEmailVerified)
            {
                return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
            }

            var token = _tokenGenerator.GenerateToken(user);
            var loginResponse = new LoginResponse(user.Id,request.Email,token);

            return Result.Success(loginResponse);
        }
    }
}