using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Response;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;
using System.Collections.Generic;

namespace ProjectManagementSystem.CQRS.Users.Queries;


public record GetAllUsersQuery : IRequest<Result<List<UserResponse>>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.Repository<User>();
        var users = await userRepository.GetAllAsync();

        if (users == null)
        {
            return Result.Failure<List<UserResponse>>(UserErrors.UserNotFound);
        }

        var userResponses = users.Select(user => new UserResponse
        {
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Status = user.Status.ToString() 
        }).ToList();

        return Result.Success(userResponses);
    }
}
