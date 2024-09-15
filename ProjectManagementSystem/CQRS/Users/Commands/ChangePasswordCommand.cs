

using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using MediatR;
using ProjectManagementSystem.Helper;
using System.ComponentModel.DataAnnotations;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Errors;

namespace ProjectManagementSystem.CQRS.Users.Commands
{
    public class ChangePasswordCommand : IRequest<Result<bool>>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
       ErrorMessage = "New password must be at least 8 characters long, and include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string NewPassword { get; set; }
    }

    public class ChangePasswordResultDto
    {
        public bool IsSuccessed { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public ChangePasswordCommandHandler(IUnitOfWork unitOfWork,IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserByEmailQuery(request.Email));

            if (user == null)
            {
                return Result.Failure<bool>(UserErrors.UserNotFound);
            }

            if (!PasswordHasher.checkPassword(request.CurrentPassword, user.Data.PasswordHash))
            {
                return Result.Failure<bool>(UserErrors.InvalidCurrentPassword);

            }

            user.Data.PasswordHash = PasswordHasher.HashPassword(request.NewPassword);
            _unitOfWork.Repository<User>().Update(user.Data);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}

