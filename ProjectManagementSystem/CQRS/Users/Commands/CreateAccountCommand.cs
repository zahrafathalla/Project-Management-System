using MediatR;
using Microsoft.AspNetCore.Identity;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.CQRS.Users.Commands
{
    public record CreateAccountCommand(
        string UserName,
        string Email,
        string Country,
        string PhoneNumber,
        string Password) : IRequest<Result<bool>>;

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateAccountCommandHandler(
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result<bool>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _mediator.Send(new CheckUserExistsQuery(request.UserName, request.UserName));

            if (userExists)
            {
                return Result.Failure<bool>(UserErrors.UserAlreadyExists);
            }

            var user = request.Map<User>();

            user.PasswordHash = PasswordHasher.HashPassword(request.Password);
            user.VerificationToken = Guid.NewGuid().ToString();

            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}
