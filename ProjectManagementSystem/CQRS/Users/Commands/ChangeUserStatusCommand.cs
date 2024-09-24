using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.CQRS.Users.Response;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Users.Commands;

public record ChangeUserStatusCommand(int UserId, UserStatus NewStatus) : IRequest<Result<bool>>;
public class ChangeUserStatusCommandHandler : IRequestHandler<ChangeUserStatusCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public ChangeUserStatusCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result<bool>> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _mediator.Send(new GetUserByIdQuery(request.UserId));

        if (!userResult.IsSuccess)
        {
            return Result.Failure<bool>(UserErrors.UserNotFound);
        }

        var user = userResult.Data;
        user.Status = request.NewStatus;

        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}
