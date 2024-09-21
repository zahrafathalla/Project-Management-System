using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification;
using ProjectManagementSystem.Repository.Specification.ProjectSpecifications;
using ProjectManagementSystem.Repository.Specification.UserSpecification;

namespace ProjectManagementSystem.CQRS.Users.Queries;

public record GetUserCountQuery(SpecParams SpecParams) : IRequest<Result<int>>;

public class GetUserCountQueryHandler : IRequestHandler<GetUserCountQuery, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserCountQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<int>> Handle(GetUserCountQuery request, CancellationToken cancellationToken)
    {
        var userSpec = new CountUserWithSpec(request.SpecParams);
        var count = await _unitOfWork.Repository<User>().GetCountWithSpecAsync(userSpec);

        return Result.Success(count);
    }
}