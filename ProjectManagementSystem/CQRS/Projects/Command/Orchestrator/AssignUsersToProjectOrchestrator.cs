using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.CQRS.Projects.Command.Orchestrator
{
    public class AssignUsersToProjectOrchestrator : IRequest<Result>
    {
        public int ProjectId { get; set; }
        public int CreatorUserId { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }

    public class AssignUsersToProjectOrchestratorHandler : IRequestHandler<AssignUsersToProjectOrchestrator, Result>
    {
        private readonly IMediator _mediator;

        public AssignUsersToProjectOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Result> Handle(AssignUsersToProjectOrchestrator request, CancellationToken cancellationToken)
        {

            var assignProjectCommand = new AssignUsersToProjectCommand
            {
                ProjectId = request.ProjectId,
                CreatorUserId = request.CreatorUserId,
                UserIds = request.UserIds,
            };
            var assignProjectResult = await _mediator.Send(assignProjectCommand, cancellationToken);

            if (!assignProjectResult.IsSuccess)
            {
                return Result.Failure(Errors.ProjectErrors.FailedToAssignUsersToTheProject);
            }
            foreach (var userId in request.UserIds.Where(id => id != request.CreatorUserId))
            {
                var sendInvitationCommand = new SendProjectInvitationCommand(request.ProjectId,userId);


                var invitationResult = await _mediator.Send(sendInvitationCommand, cancellationToken);

                if (!invitationResult.IsSuccess)
                {
                    return Result.Failure(Errors.EmailErrors.EmailSendingFailed);
                }
            }

            return Result.Success();
        }
    }
}
