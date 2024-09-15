using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;

namespace ProjectManagementSystem.CQRS.Users.Commands
{

    public class LoginCommand : IRequest<Result<LoginResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }

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
            var loginResponse = new LoginResponse
            {
                Email = request.Email,
                Token = token,
                Id = user.Id
            };

            return Result.Success(loginResponse);
        }
    }
}