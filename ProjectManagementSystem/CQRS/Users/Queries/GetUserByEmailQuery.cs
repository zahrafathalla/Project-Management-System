using MediatR;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Users.Queries
{

    public class GetUserByEmailQuery : IRequest<User>
    {
        public string Email { get; set; }
    }

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByEmailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = (await _unitOfWork.Repository<User>().GetAsync(u => u.Email == request.Email)).FirstOrDefault();
            return user;
        }
    }
}
