using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification;
using ProjectManagementSystem.Repository.Specification.ProjectSpecifications;
using ProjectManagementSystem.Repository.Specification.TaskSpecifications;

namespace ProjectManagementSystem.CQRS.Tasks.Query;

public record GetTasksQuery(SpecParams SpecParams) : IRequest<Result<IEnumerable<TaskToReturnDto>>>;

public class TaskToReturnDto
{
    public string Title { get; set; }
    public string Status { get; set; }
    public string User { get; set; }
    public string Project { get; set; }
    public DateTime DateCreated { get; set; }
}

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, Result<IEnumerable<TaskToReturnDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTasksQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<TaskToReturnDto>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var spec = new TaskSpec(request.SpecParams);

        var tasks = await _unitOfWork.Repository<WorkTask>().GetAllWithSpecAsync(spec);

        var mappedProject = tasks.Map<IEnumerable<TaskToReturnDto>>();

        return Result.Success(mappedProject);
    }
}