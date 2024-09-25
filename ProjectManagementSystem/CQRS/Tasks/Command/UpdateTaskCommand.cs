using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Tasks.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Tasks.Command
{
    public record UpdateTaskCommand(int TaskId, string? Title, TaskPriority? Priority) : IRequest<Result<bool>>;

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

            task.Title = request.Title ?? task.Title;
            task.Priority = request.Priority ?? task.Priority;


            _unitOfWork.Repository<WorkTask>().Update(task);
            await _unitOfWork.SaveChangesAsync();

            var message = $"Task '{task.Title}' has been updated in project '{task.Project.Title}' with ID {task.ProjectId}";
            await _mediator.Send(new PublishRabbitMqMessageCommand(message, "key3"));

            return Result.Success(true);
        }
    }
}
