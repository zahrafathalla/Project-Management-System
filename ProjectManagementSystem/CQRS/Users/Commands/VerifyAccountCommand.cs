using MediatR;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Users.Commands
{
    public class VerifyAccountCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class VerifyAccountCommandHandler : IRequestHandler<VerifyAccountCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public VerifyAccountCommandHandler(
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<bool> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserForVerificationQuery
            {
                Email = request.Email,
                Token = request.Token
            });

            if (user == null)
            {
                return false;
            }

            user.IsEmailVerified = true;

            _unitOfWork.Repository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }



}
