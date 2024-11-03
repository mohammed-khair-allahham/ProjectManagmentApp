using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Application.ProjectTasks.Commands;
using ProjectManagmentApp.Domain.Constants;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.TodoItems.Commands.UpdateTodoItem;

[Authorize]
[Authorize(Policy = Policies.CanUpdate)]
public record UpdateTaskCommand(int Id) : IRequest<TaskResponseDto>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? AssignedTo { get; init; }
    public PriorityLevel Priority { get; init; }
    public ProjectTaskStatus Status { get; init; }
    public int ProjectId { get; init; }
}

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _user;

    public UpdateTaskCommandHandler(IApplicationDbContext context, IMapper mapper, IUser user)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
    }

    public async Task<TaskResponseDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tasks
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);
        // Managers can update tasks, while employees can only update tasks
        // assigned to them
        if (_user.Role == Roles.Employee && entity.AssignedTo != _user.Id)
        {
            throw new UnauthorizedAccessException();
        }

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.AssignedTo = request.AssignedTo;
        entity.Priority = request.Priority;
        entity.Status = request.Status;
        entity.ProjectId = request.ProjectId;

        _context.Tasks.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        var taskDto = _mapper.Map<TaskResponseDto>(entity);

        return taskDto;
    }
}
