using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Command;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class InvitationController : BaseController
    {
        private readonly IMediator _mediator;

        public InvitationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("accept")]
        public async Task<Result> AcceptInvitation([FromQuery] int userId, [FromQuery] int projectId)
        {
            var command = new AcceptInvitationCommand
            {
                ProjectId = projectId,
                UserId = userId
            };
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPost("reject")]
        public async Task<Result> RejectInvitation([FromQuery] int userId, [FromQuery] int projectId)
        {
            var command = new RejectInvitationCommand
            {
                ProjectId = projectId,
                UserId = userId
            };

            var result = await _mediator.Send(command);

            return result;
        }
    }
}
