using MediatR;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Users.Queries
{
    public class GetUserForVerificationQuery : IRequest<User>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class GetUserForVerificationQueryHandler : IRequestHandler<GetUserForVerificationQuery, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserForVerificationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(GetUserForVerificationQuery request, CancellationToken cancellationToken)
        {
            var user = (await _unitOfWork.Repository<User>()
                .GetAsync(u => u.Email == request.Email &&
                               u.VerificationToken == request.Token &&
                               !u.IsEmailVerified)).FirstOrDefault();

            return user;
        }
    }
}
