using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification.UserSpecification;


namespace ProjectManagementSystem.CQRS.Users.Queries
{
    public record GetUserByEmailQuery(string Email) : IRequest<Result<User>>;

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, Result<User>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByEmailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return Result.Failure<User>(UserErrors.InvalidEmail);
            }

            var spec = new UserSpecWithUserRoles(request.Email);
            var users = await _unitOfWork.Repository<User>().GetAllWithSpecAsync(spec);
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return Result.Failure<User>(UserErrors.UserNotFound);
            }

            return Result.Success(user);
        }
    }
}

