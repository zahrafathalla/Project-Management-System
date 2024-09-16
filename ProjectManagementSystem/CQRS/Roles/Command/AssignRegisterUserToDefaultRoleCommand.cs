﻿using MediatR;
using ProjectManagementSystem.Abstractions.Consts;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.CQRS.Roles.Command;

public record AssignRegisterUserToDefaultRoleCommand (User user) : IRequest<bool>;

public class AssignRegisterUserToDefaultRoleHandler : IRequestHandler<AssignRegisterUserToDefaultRoleCommand, bool>
{
    private readonly IMediator _mediator;

    public AssignRegisterUserToDefaultRoleHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> Handle(AssignRegisterUserToDefaultRoleCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddRoleToUserCommand (request.user,DefaultRoles.Member));

        return true;
    }
}
