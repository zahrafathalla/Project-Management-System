using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Command;
using ProjectManagementSystem.CQRS.Projects.Command.Orchestrator;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.DTO;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Specification;
using ProjectManagementSystem.ViewModel;
using System.Security.Claims;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly UserState _userState;

        public ProjectController(IMediator mediator, UserState userState)
        {
            _mediator = mediator;
            _userState = userState;
        }

        [HttpGet("view-project/{projectId}")]
        public async Task<Result<ProjectToReturnDto>> GetProjectById(int projectId)
        {
            var result = await _mediator.Send(new GetProjectByIdQuery(projectId));
            if (!result.IsSuccess)
            {
                return Result.Failure<ProjectToReturnDto>(result.Error);
            }
            var projectToReturnDto = result.Data.Map<ProjectToReturnDto>();

            return Result.Success(projectToReturnDto);
        }

        [HttpGet("List-Projects")]
        public async Task<Result<Pagination<ProjectToReturnDto>>> GetAllProjects([FromQuery] SpecParams spec)
        {
            var result = await _mediator.Send(new GetProjectsQuery(spec));
            if (!result.IsSuccess)
            {
                return Result.Failure<Pagination<ProjectToReturnDto>>(result.Error);
            }

            var projectCount = await _mediator.Send(new GetProjectCountQuery(spec));
            var paginationResult = new Pagination<ProjectToReturnDto>(spec.PageSize, spec.PageIndex, projectCount.Data , result.Data);
            return Result.Success(paginationResult);
        }

        [HttpPost("create-project")]
        [Authorize]
        public async Task<Result<int>> CreateProject([FromBody] AddProjectViewModel viewModel)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var command = await _mediator.Send(new AddProjectCommand(viewModel.Title, viewModel.Description, userId));

            return command;
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