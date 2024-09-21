using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Projects.Command
{
    public record SendProjectInvitationCommand(int ProjectId , int UserId) : IRequest<Result>;

    public class SendProjectInvitationCommandHandler : IRequestHandler<SendProjectInvitationCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SendProjectInvitationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(SendProjectInvitationCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId);
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(request.UserId);

            if (project == null )
                return Result.Failure(Errors.ProjectErrors.ProjectNotFound);
            

            if (user == null )
                return Result.Failure(Errors.UserErrors.UserNotFound);

            var subject = $"Invitation to join project {project.Title}";
            var body = $"Hello {user.UserName},<br><br>" +
                       $"You have been invited to join the project <b>{project.Title}</b>. ";

            var emailSent = await EmailSender.SendEmailAsync(user.Email, subject, body);

            if (!emailSent)
                return Result.Failure(Errors.EmailErrors.EmailSendingFailed);
            

            return Result.Success();
        }
    
    }
}
