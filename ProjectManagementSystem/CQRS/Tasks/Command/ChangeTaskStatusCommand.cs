using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Tasks.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Tasks.Command
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

            var message = $"Task '{task.Title}' has been updated in project '{task.Project.Title}' with ID {task.ProjectId}";
            await _mediator.Send(new PublishRabbitMqMessageCommand(message, "key3"));

            return Result.Success(true);
        }
    }
}
