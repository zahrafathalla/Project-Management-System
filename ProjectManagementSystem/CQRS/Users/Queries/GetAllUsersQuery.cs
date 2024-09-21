using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Response;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification;
using ProjectManagementSystem.Repository.Specification.ProjectSpecifications;
using ProjectManagementSystem.Repository.Specification.UserSpecification;
using System.Collections.Generic;

namespace ProjectManagementSystem.CQRS.Users.Queries;


public record GetAllUsersQuery(SpecParams SpecParams) : IRequest<Result<IEnumerable<UserResponse>>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserSpec(request.SpecParams);
        var users = await _unitOfWork.Repository<User>().GetAllWithSpecAsync(spec);

        if (users == null)
        {
            return Result.Failure<IEnumerable<UserResponse>>(UserErrors.UserNotFound);
        }

        var userResponses = users.Select(user => new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Status = user.Status.ToString() 
        }).AsEnumerable();

        return Result.Success(userResponses);
    }
}
