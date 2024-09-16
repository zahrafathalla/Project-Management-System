
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using MediatR;
using ProjectManagementSystem.Helper;
using System.ComponentModel.DataAnnotations;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Errors;

namespace ProjectManagementSystem.CQRS.Users.Commands;

public record ChangePasswordCommand(
                string Email,
                string CurrentPassword,
                string NewPassword) : IRequest<Result<bool>>;


public class ChangePasswordResultDto
{
    public bool IsSuccessed { get; set; }
    public string ErrorMessage { get; set; }
}

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _mediator.Send(new GetUserByEmailQuery(request.Email));

        if (!userResult.IsSuccess)
        {
            return Result.Failure<bool>(UserErrors.UserNotFound);
        }

        var user = userResult.Data;

        if (!PasswordHasher.checkPassword(request.CurrentPassword, user.PasswordHash))
        {
            return Result.Failure<bool>(UserErrors.InvalidCurrentPassword);
        }

        user.PasswordHash = PasswordHasher.HashPassword(request.NewPassword);
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}
