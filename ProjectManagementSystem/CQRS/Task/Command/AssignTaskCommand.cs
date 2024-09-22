using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.CQRS.Task.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Task.Command;

public record AssignTaskCommand(int taskId, int userId) : IRequest<Result>;

public class AssignTaskCommandHandler : IRequestHandler<AssignTaskCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public AssignTaskCommandHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
    {

        var taskResult = await _mediator.Send(new GetTaskByIdQuery(request.taskId));
        if (!taskResult.IsSuccess) 
        {
            return Result.Failure(TaskErrors.TaskNotFound);
        }
        var isAssignedResult = (await _mediator.Send(new CheckUserAssignedToTaskQuery(request.userId, request.taskId))).Data;

        if (isAssignedResult)
        {
            return Result.Failure(TaskErrors.TaskAlreadyAssigned); 
        }

        var task = taskResult.Data;
        var isUserAssignedToProject = (await _mediator.Send(new CheckUserAssignedToProjectQuery(request.userId, task.Project.Id))).Data;
        if (!isUserAssignedToProject)
        {
                return Result.Failure<int>(ProjectErrors.UserIsNotAssignedToThisProject);
        }
        
        task.AssignedToUserId = request.userId;

        _unitOfWork.Repository<WorkTask>().Update(task);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
