using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Response;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Users.Queries;

public record GetUserByIdQuery(int UserId) : IRequest<Result<User>>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = (await _unitOfWork.Repository<User>().GetAsync(u => u.Id == request.UserId && !u.IsDeleted)).FirstOrDefault();

        if (user == null)
        {
            return Result.Failure<User>(UserErrors.UserNotFound);
        }

        return Result.Success(user);
    }
}