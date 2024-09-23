using MediatR;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Task.Query;

public record GetTasksByProjectIdQuery(int ProjectId) : IRequest<TaskListResponse>;

public class TaskListResponse
{
    public List<WorkTask> ToDo { get; set; } = new();
    public List<WorkTask> InProgress { get; set; } = new();
    public List<WorkTask> Done { get; set; } = new();
}

public class GetTasksByProjectIdQueryHandler : IRequestHandler<GetTasksByProjectIdQuery, TaskListResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTasksByProjectIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskListResponse> Handle(GetTasksByProjectIdQuery request, CancellationToken cancellationToken)
    {
        var tasks = (await _unitOfWork.Repository<WorkTask>().GetAsync(t => t.ProjectId == request.ProjectId));

        var response = new TaskListResponse();

        foreach (var task in tasks)
        {
            switch (task.Status)
            {
                case WorkTaskStatus.ToDo:
                    response.ToDo.Add(task);
                    break;
                case WorkTaskStatus.InProgress:
                    response.InProgress.Add(task);
                    break;
                case WorkTaskStatus.Done:
                    response.Done.Add(task);
                    break;
            }
        }

        return response;
    }
}