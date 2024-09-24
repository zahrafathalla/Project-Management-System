using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Task.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Task.Command
{
    public record UpdateTaskCommand(int TaskId, string Title, TaskPriority priority) : IRequest<Result<bool>>;

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateTaskCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var taskResult = await _mediator.Send(new GetTaskByIdQuery(request.TaskId));

            if (!taskResult.IsSuccess)
            {
                return Result.Failure<bool>(TaskErrors.TaskNotFound);
            }

            var task = taskResult.Data;
            task.Title = request.Title;
            task.Priority = request.priority;

            _unitOfWork.Repository<WorkTask>().Update(task);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}
