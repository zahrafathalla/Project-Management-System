using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Projects.Query;


public record projectToReturnDto(string title, string status, int numberofUsers, int numoftasks, DateTime Datecreated);

public class GetAllProjectsQuery : IRequest<Result<List<projectToReturnDto>>>
{
    public int Skip { get; set; }
    public int Take { get; set; }
    public string? SearchTerm { get; set; }

    public GetAllProjectsQuery(int skip, int take, string? searchTerm)
    {
        Skip = skip;
        Take = take;
        SearchTerm = searchTerm;
    }
}
public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, Result<List<projectToReturnDto>>>
{
    private readonly IProjectRepository _projectRepository;

    public GetAllProjectsQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<List<projectToReturnDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllAsync(request.Skip, request.Take,request.SearchTerm);

        var projectToReturnDto = projects.Select(p =>
                                                    new projectToReturnDto(p.Title,
                                                                           p.Status.ToString(),
                                                                           p.UserProjects.Count(),
                                                                           p.Tasks.Count(),
                                                                           p.DateCreated)
                                                    ).ToList();

        return Result.Success(projectToReturnDto);
    }

}


