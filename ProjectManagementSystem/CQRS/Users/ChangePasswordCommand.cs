

using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using MediatR;
using ProjectManagementSystem.Helper;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.CQRS.Users.Commands
{
    public class ChangePasswordCommand : IRequest<ChangePasswordResultDto>
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

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResultDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ChangePasswordResultDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Repository<User>().GetAsync(u => u.Email == request.Email);
           var user = users.FirstOrDefault();

            if (user == null)
            {
                return new ChangePasswordResultDto
                {
                    IsSuccessed = false,
                    ErrorMessage = "User not found"
                };
            }

            if (!PasswordHasher.checkPassword(request.CurrentPassword, user.PasswordHash))
            {
                return new ChangePasswordResultDto
                {
                    IsSuccessed = false,
                    ErrorMessage = "Current password is incorrect"
                };
            }

            user.PasswordHash = PasswordHasher.HashPassword(request.NewPassword);
            _unitOfWork.Repository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return new ChangePasswordResultDto
            {
                IsSuccessed = true
            };
        }
    }
}

