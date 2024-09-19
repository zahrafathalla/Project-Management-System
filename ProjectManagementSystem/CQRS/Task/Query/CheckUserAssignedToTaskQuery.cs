using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Task.Query;

public record CheckUserAssignedToTaskQuery(int UserId, int TaskId) : IRequest<Result<bool>>;

public class CheckUserAssignedToTaskQueryHandler : IRequestHandler<CheckUserAssignedToTaskQuery, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckUserAssignedToTaskQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(CheckUserAssignedToTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Repository<WorkTask>()
            .GetByIdAsync(request.TaskId);

        if (task is null)
        {
            return Result.Failure<bool>(TaskErrors.TaskNotFound);
        }

        return Result.Success(task.AssignedToUserId == request.UserId);
    }
}

