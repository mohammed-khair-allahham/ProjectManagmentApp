using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Application.ProjectTasks.Commands;
using ProjectManagmentApp.Application.ProjectTasks.Commands.CreateTask;
using ProjectManagmentApp.Application.ProjectTasks.Commands.DeleteTask;
using ProjectManagmentApp.Application.ProjectTasks.Queries.GetTaskDetails;
using ProjectManagmentApp.Application.ProjectTasks.Queries.GetTaskList;
using ProjectManagmentApp.Application.TodoItems.Commands.UpdateTodoItem;

namespace ProjectManagmentApp.Web.Endpoints;

public class Tasks : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(TaskListAsync)
            .MapGet(TaskGetAsync, "{id}")
            .MapPost(TaskCreateAsync)
            .MapPut(TaskUpdateAsync, "{id}")
            .MapPut(TaskUpdateStatusAsync, "status/{id}")
            .MapDelete(DeleteAsync, "{id}");

    }

    public async Task<TaskDetailsDto> TaskGetAsync(ISender sender, int id)
        => await sender.Send(new GetTaskDetailsQuery(id));

    public async Task<PaginatedList<GetTaskListDto>> TaskListAsync(ISender sender, [AsParameters] GetTaskListWithPaginationQuery query)
        => await sender.Send(query);

    public async Task<TaskResponseDto> TaskCreateAsync(ISender sender, CreateTaskCommand command)
        => await sender.Send(command);

    public async Task<TaskResponseDto> TaskUpdateAsync(ISender sender, int id, UpdateTaskCommand command)
        => await sender.Send(command);

    public async Task<IResult> TaskUpdateStatusAsync(ISender sender, int id, UpdateTaskStatusCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteAsync(ISender sender, int id)
    {
        await sender.Send(new DeleteTaskCommand(id));
        return Results.NoContent();
    }
}
