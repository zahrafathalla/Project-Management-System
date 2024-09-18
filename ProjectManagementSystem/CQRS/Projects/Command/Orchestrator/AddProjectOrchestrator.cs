using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Errors;

namespace ProjectManagementSystem.CQRS.Projects.Command.Orchestrator
{
    public class AddProjectOrchestrator : IRequest<Result<int>>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedByUserId { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }

    public class AddProjectOrchestratorHandler : IRequestHandler<AddProjectOrchestrator, Result<int>>
    {
        private readonly IMediator _mediator;

        public AddProjectOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<int>> Handle(AddProjectOrchestrator request, CancellationToken cancellationToken)
        {
            var addProjectCommand = new AddProjectCommand
            {
                Title = request.Title,
                Description = request.Description,
                CreatedByUserId = request.CreatedByUserId
            };

            var addProjectResult = await _mediator.Send(addProjectCommand, cancellationToken);

            if (!addProjectResult.IsSuccess)
            {
                return Result.Failure<int>(ProjectErrors.ProjectCreationFailed);
            }

            var projectId = addProjectResult.Data;

            var assignUsersCommand = new AssignUsersToProjectCommand
            {
                ProjectId = projectId,
                UserIds = request.UserIds
            };

            var assignUsersResult = await _mediator.Send(assignUsersCommand, cancellationToken);

            if (!assignUsersResult.IsSuccess)
            {
                return Result.Failure<int>(ProjectErrors.UserAssignmentFailed);
            }

            return Result.Success(projectId);
        }
    }
    
}
