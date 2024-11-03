using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Application.Projects.Commands;
using ProjectManagmentApp.Application.Projects.Commands.CreateProject;
using ProjectManagmentApp.Application.Projects.Commands.DeleteProject;
using ProjectManagmentApp.Application.Projects.Commands.UpdateProject;
using ProjectManagmentApp.Application.Projects.Queries.GetProjectDetails;
using ProjectManagmentApp.Application.Projects.Queries.GetProjectList;

namespace ProjectManagmentApp.Web.Endpoints;

public class Projects : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(ListAsync)
            .MapGet(GetAsync, "{id}")
            .MapPost(CreateAsync)
            .MapPut(UpdateAsync, "{id}")
            .MapDelete(DeleteAsync, "{id}");

    }

    public async Task<ProjectDetailsDto> GetAsync(ISender sender, int id)
        => await sender.Send(new GetProjectDetailsQuery(id));

    public async Task<PaginatedList<GetProjectListDto>> ListAsync(ISender sender, [AsParameters] GetProjectListWithPaginationQuery query)
        => await sender.Send(query);

    public async Task<ProjectResponseDto> CreateAsync(ISender sender, CreateProjectCommand command)
        => await sender.Send(command);

    public async Task<ProjectResponseDto> UpdateAsync(ISender sender, int id, UpdateProjectCommand command)
        => await sender.Send(command);

    public async Task<IResult> DeleteAsync(ISender sender, int id)
    {
        await sender.Send(new DeleteProjectCommand(id));
        return Results.NoContent();
    }
}
