using MediatR;
using ProjectManagementSystem.Abstractions.Consts;
using ProjectManagementSystem.CQRS.Users.Queries;

namespace ProjectManagementSystem.CQRS.Roles.Command;

public class AssignRegisterUserToDefaultRoleCommand : IRequest<bool>
{
    public int UserId { get; set; }
    public string RoleName { get; set; }
}

public class AssignRegisterUserToDefaultRoleHandler : IRequestHandler<AssignRegisterUserToDefaultRoleCommand, bool>
{
    private readonly IMediator _mediator;

    public AssignRegisterUserToDefaultRoleHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> Handle(AssignRegisterUserToDefaultRoleCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddRoleToUserCommand { UserId = request.UserId, RoleName = DefaultRoles.Member });

        return true;
    }
}
