using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Abstractions.Consts;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.CQRS.Roles.Command;

public record AssignRegisteredUserToDefaultRoleCommand (int userId) : IRequest<Result<bool>>;

public class AssignRegisterUserToDefaultRoleHandler : IRequestHandler<AssignRegisteredUserToDefaultRoleCommand, Result<bool>>
{
    private readonly IMediator _mediator;

    public AssignRegisterUserToDefaultRoleHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<bool>> Handle(AssignRegisteredUserToDefaultRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddRoleToUserCommand (request.userId,DefaultRoles.Member));

        return result;
    }
}
