using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.CQRS.Projects.Query;

namespace ProjectManagementSystem.CQRS.Projects.Command
{
    public class AcceptInvitationCommand : IRequest<Result>
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }

    public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public AcceptInvitationCommandHandler(
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            var userProject = await _mediator.Send(new GetUsersInProjectByProjectIdQuery(request.UserId, request.ProjectId));

            if (userProject == null)
            {
                return Result.Failure(ProjectErrors.UserIsNotAssignedToThisProject);
            }

            if (userProject.Status == InvitationStatus.Accepted)
            {
                return Result.Failure(EmailErrors.InvitationAlreadyAccepted);
            }

            userProject.Status = InvitationStatus.Accepted;
            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Invitation accepted");
        }
    }
}
