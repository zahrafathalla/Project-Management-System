using MediatR;
using ProjectManagementSystem.Abstractions.Consts;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.CQRS.Roles.Command;

public record AssignRegisteredUserToDefaultRoleCommand (int userId) : IRequest<bool>;

public class AssignRegisterUserToDefaultRoleHandler : IRequestHandler<AssignRegisteredUserToDefaultRoleCommand, bool>
{
    private readonly IMediator _mediator;

    public AssignRegisterUserToDefaultRoleHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> Handle(AssignRegisteredUserToDefaultRoleCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddRoleToUserCommand (request.userId,DefaultRoles.Member));

        return true;
    }
}
