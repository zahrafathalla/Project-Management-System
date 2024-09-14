using MediatR;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using System.Runtime.CompilerServices;

namespace ProjectManagementSystem.CQRS.Users.Queries
{
    public class CheckUserExistsQuery :IRequest<bool>
    {
        public string UserName { get; set; }
        public string Email {  get; set; }
    }

    public class CheckUserExistsQueryHandler : IRequestHandler<CheckUserExistsQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckUserExistsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(CheckUserExistsQuery request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.Repository<User>()
                            .GetAsync(u=>u.Email == request.Email || u.UserName == request.UserName);

            return existingUser.Any();

        }
    }
}
