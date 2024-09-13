using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Contract;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.User.Queries;

public class GetTokenQuery : IRequest<Result<AuthResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, Result<AuthResponse>>
{
    private readonly IGenericRepository<Data.Entities.User> _repository;
    private readonly IMediator _mediator;

    public GetTokenQueryHandler(IGenericRepository<Data.Entities.User> repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<Result<AuthResponse>> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        var users = await _repository.GetAsync(u => u.Email == request.Email && !u.IsDeleted&&u.IsEmailVerified);
        var user = users.FirstOrDefault();

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var isValidPassword = true; // Implement actual password validation

        if (!isValidPassword)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        
        var (token, expiresIn) = await _mediator.Send(new GenerateTokenQuery { User = user });

        var response = new AuthResponse(user.Id, user.Email, user.UserName, user.UserName, token, expiresIn);

        return Result.Success(response);
    }
}

