using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Command;
using ProjectManagementSystem.CQRS.Projects.Command.Orchestrator;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.Data.Entities;
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

        [HttpGet("view-project/{projectId}")]
        public async Task<Result<Project>> GetProjectById(int projectId)
        {
            var result = await _mediator.Send(new GetProjectByIdQuery(projectId));

            return result; // should return Dto instead of project. Waiting for al- Zahra's approval on the DTO format
        }

        [HttpGet("List-Projects")]
        public async Task<ActionResult<List<projectToReturnDto>>> GetAllProjects(string? SearchTerm, [FromQuery] int skip = 0 , [FromQuery] int take = 10 )
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery(skip, take, SearchTerm));
            return Ok(projects);
        }


        [HttpPost("create-project")]
        public async Task<Result<int>> CreateProject([FromBody] AddProjectViewModel viewModel)
        {
            var command = viewModel.Map<AddProjectOrchestrator>();
            var result = await _mediator.Send(command);

            return result;
        }

        [HttpPut("Update-project/{projectId}")]
        public async Task<Result<bool>> UpdateProject([FromBody] UpdateProjectViewModel viewModel)
        {
            var command = viewModel.Map<UpdateProjectCommand>();
            var result = await _mediator.Send(command);

            return result;
        }

        [HttpDelete("Delete-project/{projectId}")]
        public async Task<Result<bool>> DeleteProject(int projectId)
        {
            var result = await _mediator.Send(new DeleteProjectCommand(projectId));

            return result;
        }
    }
}