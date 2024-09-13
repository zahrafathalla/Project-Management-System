using MediatR;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.CQRS.Users.Commands
{
    public class CreateAccountCommand : IRequest<CreateAccountToReturnDto>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
       ErrorMessage = "Password must be at least 8 characters long, and include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
    }

    public class CreateAccountToReturnDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsSuccessed { get; set; } = true;
        public string ErrorMessage { get; set; }
    }
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CreateAccountToReturnDto>
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
        public async Task<CreateAccountToReturnDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _mediator.Send(new CheckUserExistsQuery
            {
                Email = request.Email,
                UserName = request.UserName
            });

            if (userExists)
            {
                return new CreateAccountToReturnDto
                {
                    IsSuccessed = false,
                    ErrorMessage = "A user with this email or username already exists"
                };
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Country = request.Country,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                IsEmailVerified = false
            };

            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var createAccountToReturnDto = user.Map<CreateAccountToReturnDto>();

            return createAccountToReturnDto;
        }
    }
}
