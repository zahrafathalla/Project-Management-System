using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Task.Command;

public record CreateTaskCommand(string Title, int ProjectId, int? AssignedToUserId) : IRequest<Result<int>>;

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

        if (request.AssignedToUserId.HasValue)
        {
            var isUserAssignedToProject = (await _mediator.Send(new CheckUserAssignedToProjectQuery(request.AssignedToUserId.Value, request.ProjectId))).Data;
            if (!isUserAssignedToProject)
            {
                return Result.Failure<int>(ProjectErrors.UserIsNotAssignedToThisProject);
            }
        }

        var task = request.Map<WorkTask>();

        await _unitOfWork.Repository<WorkTask>().AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(task.Id);
    }
}