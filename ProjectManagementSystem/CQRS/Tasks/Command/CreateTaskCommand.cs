using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Tasks.Command;

public record CreateTaskCommand(string Title, int ProjectId, int? AssignedToUserId, TaskPriority Priority) : IRequest<Result<int>>;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public CreateTaskCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result<int>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var projectResult = await _mediator.Send(new GetProjectByIdQuery(request.ProjectId));

        if (!projectResult.IsSuccess)
        {
            return Result.Failure<int>(ProjectErrors.ProjectNotFound);
        }

        var task = request.Map<WorkTask>();


        if (request.AssignedToUserId.HasValue)
        {           
            var isUserAssignedToProject = (await _mediator.Send(new CheckUserAssignedToProjectQuery(request.AssignedToUserId.Value, request.ProjectId))).Data;
            if (!isUserAssignedToProject)
            {
                return Result.Failure<int>(ProjectErrors.UserIsNotAssignedToThisProject);
            }
            var userResult = await _mediator.Send(new GetUserByIdQuery(request.AssignedToUserId.Value));
            if (!userResult.IsSuccess)
            {
                return Result.Failure<int>(UserErrors.UserNotFound);
            }

            task.AssignedToUser = userResult.Data;
        }

        await _unitOfWork.Repository<WorkTask>().AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        var CreatedMessage = $"Task '{task.Title}' has been created in project '{projectResult.Data.Title}' with ID {task.ProjectId}";
        await _mediator.Send(new PublishRabbitMqMessageCommand(CreatedMessage, "key2"));


        if (request.AssignedToUserId.HasValue)
        {
            var AssignmentMessage = $"Task '{task.Title}' has been assigned to user with email {task.AssignedToUser.Email}";
            await _mediator.Send(new PublishRabbitMqMessageCommand(AssignmentMessage, "key1"));
        }

        return Result.Success(task.Id);
    }
}