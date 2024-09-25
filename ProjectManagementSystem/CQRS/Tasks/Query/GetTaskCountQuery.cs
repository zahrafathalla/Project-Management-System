using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification.ProjectSpecifications;
using ProjectManagementSystem.Repository.Specification;
using ProjectManagementSystem.Repository.Specification.TaskSpecifications;

namespace ProjectManagementSystem.CQRS.Tasks.Query;

public record GetTaskCountQuery(SpecParams SpecParams) : IRequest<Result<int>>;

public class GetTaskCountQueryHandler : IRequestHandler<GetTaskCountQuery, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTaskCountQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<int>> Handle(GetTaskCountQuery request, CancellationToken cancellationToken)
    {
        var taskSpec = new CountTaskWithSpec(request.SpecParams);
        var count = await _unitOfWork.Repository<WorkTask>().GetCountWithSpecAsync(taskSpec);

        return Result.Success(count);
    }
}