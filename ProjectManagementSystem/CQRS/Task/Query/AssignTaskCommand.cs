using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Task.Query;

public record AssignTaskCommand(int taskId, int userId) : IRequest<Result>;

public class AssignTaskCommandHandler : IRequestHandler<AssignTaskCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Repository<WorkTask>().GetByIdAsync(request.taskId);
        if (task == null)
        {
            return Result.Failure(TaskErrors.TaskNotFound);
        }

        task.AssignedToUserId = request.userId;

        _unitOfWork.Repository<WorkTask>().Update(task);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
