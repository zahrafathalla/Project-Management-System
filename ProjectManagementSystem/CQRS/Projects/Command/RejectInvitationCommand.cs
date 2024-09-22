using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.CQRS.Projects.Query;

namespace ProjectManagementSystem.CQRS.Projects.Command
{
    public class RejectInvitationCommand : IRequest<Result>
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }

    public class RejectInvitationCommandHandler : IRequestHandler<RejectInvitationCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public RejectInvitationCommandHandler(
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result> Handle(RejectInvitationCommand request, CancellationToken cancellationToken)
        {
            var userProject = await _mediator.Send(new GetUserProjectQuery(request.ProjectId, request.UserId));

            if (userProject == null)
            {
                return Result.Failure(ProjectErrors.UserIsNotAssignedToThisProject);
            }
           
            if (userProject.Status == InvitationStatus.Rejected)
            {
                return Result.Failure(EmailErrors.InvitationAlreadyRejected);
            }

            userProject.Status = InvitationStatus.Rejected;
            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Invitation rejected");
        }

    }
}
