using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Task.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Task.Command
{
    public record ChangeTaskStatusCommand(int TaskId, WorkTaskStatus NewStatus) : IRequest<Result<bool>>;

    public class ChangeTaskStatusCommandHandler : IRequestHandler<ChangeTaskStatusCommand, Result<bool>>
    {

        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeTaskStatusCommandHandler(
            IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {

            var taskResult = await _mediator.Send(new GetTaskByIdQuery(request.TaskId));

            if (taskResult == null)
            {
                return Result.Failure<bool>(TaskErrors.TaskNotFound);
            }

            var task = taskResult.Data;
            task.Status = request.NewStatus;

            _unitOfWork.Repository<WorkTask>().Update(task);
             await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}
