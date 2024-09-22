using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Response;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification;
using ProjectManagementSystem.Repository.Specification.ProjectSpecifications;
using ProjectManagementSystem.Repository.Specification.UserSpecification;
using System.Collections.Generic;

namespace ProjectManagementSystem.CQRS.Users.Queries;


public record GetAllUsersQuery(SpecParams SpecParams) : IRequest<Result<IEnumerable<UserToReturnDto>>>;

public class UserToReturnDto ()
{
    public string UserName { get; set; }
    public string Status { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime DateCreated { get; set; }
}
public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserToReturnDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<UserToReturnDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserSpec(request.SpecParams);
        var users = await _unitOfWork.Repository<User>().GetAllWithSpecAsync(spec);

        if (users == null)
        {
            return Result.Failure<IEnumerable<UserToReturnDto>>(UserErrors.UserNotFound);
        }

        var mappedUser = users.Map<IEnumerable<UserToReturnDto>>();

        return Result.Success(mappedUser);
    }
}
