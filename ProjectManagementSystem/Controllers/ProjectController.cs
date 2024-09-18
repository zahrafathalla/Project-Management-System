using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Command.Orchestrator;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.ViewModel;

namespace ProjectManagementSystem.Controllers
{

    public class ProjectController : BaseController
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("create-project")]
        public async Task<Result<int>> CreateProject([FromBody] AddProjectViewModel request)
        {
            var command = request.Map<AddProjectOrchestrator>();
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Result.Success(result.Data);
            }

            return Result.Failure<int>(ProjectErrors.ProjectCreationFailed);

        }
    }
}
